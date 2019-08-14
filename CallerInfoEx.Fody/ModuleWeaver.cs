using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fody;
using Mono.Cecil;
using Mono.Cecil.Cil;
using Mono.Cecil.Rocks;

namespace CallerInfoEx.Fody
{
    public class ModuleWeaver : BaseModuleWeaver
    {
        public override void Execute()
        {
            if (Config != null)
            {
                var FunctionNamespace = Config.Attribute("Namespace").Value;
                var FunctionParameterAttribute = Config.Attribute("Attribute").Value;
                var rngset = new HashSet<long>();
                var rng = System.Security.Cryptography.RandomNumberGenerator.Create();
                var bytes = new byte[64];
                var nullableulongconstructor = typeof(ulong?).GetConstructor(new[] { typeof(ulong) });
                var allmethods = this.ModuleDefinition.GetAllTypes().SelectMany(x => x.Methods.AsEnumerable()).Where(x => x.HasBody).Where(x => x.Body.Instructions.Any(p => p.OpCode == OpCodes.Callvirt));
                var allinstructions = allmethods.ToDictionary(t => t, X => X.Body.Instructions.Where(x => x.OpCode == OpCodes.Callvirt && (x.Operand as MethodReference).Resolve().HasParameters).Where(x => (x.Operand as MethodReference).Resolve().DeclaringType.Namespace == FunctionNamespace).Where(x => (x.Operand as MethodReference).Resolve().Parameters.Any(t => t.HasCustomAttributes)).Where(x => (x.Operand as MethodReference).Resolve().Parameters.Any(t => t.CustomAttributes.Any(p => p.AttributeType.Name == FunctionParameterAttribute))).Reverse());
                var ils = new List<Instruction>();
                foreach (var methodinstructions in allinstructions)
                {
                    var method = methodinstructions.Key;
                    var IL = method.Body.GetILProcessor();
                    //method.Body.SimplifyMacros();
                    if (methodinstructions.Value.Count() > 0)
                    {
                        var methodclass = method.DeclaringType.DeclaringType;
                        var instructionpoints = new List<Instruction>();

                        foreach (var ILinstruction in methodinstructions.Value)
                        {
                            var ins = method.Body.Instructions.IndexOf(ILinstruction);
                            var methoddeclaration = (ILinstruction.Operand as MethodReference).Resolve();
                            var parameterindexes = methoddeclaration.Parameters.Where(x => x.HasCustomAttributes).Where(x => x.CustomAttributes.Any(p => p.AttributeType.Name == FunctionParameterAttribute)).Select(x => methoddeclaration.Parameters.IndexOf(x));

                            while (!(method.Body.Instructions[ins].Previous.OpCode == OpCodes.Nop))
                            {
                                if (method.Body.Instructions[ins].Previous.OpCode == OpCodes.Ldloc_1
                                 | method.Body.Instructions[ins].Previous.OpCode == OpCodes.Ldloc_2
                                | method.Body.Instructions[ins].Previous.OpCode == OpCodes.Ldloc_3
                                | method.Body.Instructions[ins].Previous.OpCode == OpCodes.Ldloc_S)
                                {
                                    if (method.Body.Instructions[ins].Previous.Previous.OpCode == OpCodes.Initobj)
                                    {
                                        if ((method.Body.Instructions[ins].Previous.Previous.Operand as TypeReference).Resolve() == ModuleDefinition.ImportReference(typeof(ulong?)).Resolve())
                                        {
                                            if (method.Body.Instructions[ins].Previous.Previous.Previous.OpCode == OpCodes.Ldloca_S
                                            || (method.Body.Instructions[ins].Previous.Previous.Previous.OpCode == OpCodes.Ldloca))
                                            {
                                                ils.Add(method.Body.Instructions[ins]);
                                                //instructionpoints.Add(method.Body.Instructions[ins]);
                                                {
                                                    var instruction = method.Body.Instructions[ins];
                                                    var methodref = (instruction.Operand as MethodReference);
                                                    rng.GetBytes(bytes, 0, 64);
                                                    var randomnumber = BitConverter.ToInt64(bytes, 0);
                                                    while (rngset.Add(randomnumber))
                                                    {
                                                        randomnumber = BitConverter.ToInt64(bytes, 0);
                                                    }
                                                    randomnumber = BitConverter.ToInt64(bytes, 0);
                                                    var IL0 = IL.Create(OpCodes.Ldc_I8, randomnumber);
                                                    var IL1 = IL.Create(OpCodes.Newobj, ModuleDefinition.ImportReference(nullableulongconstructor));
                                                    if (method.Body.Variables.Contains(instruction.Previous.Operand as VariableReference))
                                                    {
                                                        //localvarindex = method.Body.Variables.IndexOf((instruction.Previous.Operand as VariableReference).Resolve());
                                                        // method.Body.Variables.Remove((instruction.Previous.Operand as VariableReference).Resolve());
                                                    }
                                                    IL.Remove(instruction.Previous);
                                                    IL.Remove(instruction.Previous);
                                                    IL.Remove(instruction.Previous);
                                                    IL.InsertBefore(instruction, IL0);
                                                    IL.InsertBefore(instruction, IL1);
                                                }

                                            }
                                        }
                                    }
                                }
                                ins = method.Body.Instructions.IndexOf(method.Body.Instructions[ins].Previous);
                            }
                            instructionpoints.Add(ILinstruction);
                        }
                        File.WriteAllLines("ils.txt", ils.Select(x => x.ToString()));
                        var localvarindex = 0;
                        foreach (var instruction in instructionpoints)
                        {
                            
                        }

                    }
                }
            }
        }


        public override IEnumerable<string> GetAssembliesForScanning()
        {
            yield return "mscorlib";
            yield return "netstandard";
        }

        public override bool ShouldCleanReference => true;
    }
}
