using System;
using System.Collections.Generic;
using System.Linq;
using Mono.Cecil;
using System.Text;
using System.Threading.Tasks;

namespace CSharpReferenceCreator
{
				internal static class Helper
				{
								public static Mono.Collections.Generic.Collection<TypeDefinition> GetTypesFromFile(string filePath)
								{
												Console.WriteLine(string.Format("Checking for '{0}'", filePath));
												if (!System.IO.File.Exists(filePath))
												{
																Console.WriteLine("File not Found.");
																Console.ReadKey();
																return null;
												}

												Console.WriteLine(string.Format("Loading '{0}'", filePath));
												AssemblyDefinition assemby;
												try
												{
																assemby = AssemblyDefinition.ReadAssembly(filePath);
												}
												catch (Exception e)
												{
																Console.WriteLine("Loading failed");
																Console.Write(e.ToString());
																Console.ReadKey();
																return null;
												}
												Console.WriteLine(string.Format("Get Types from '{0}'", filePath));

												Mono.Collections.Generic.Collection<TypeDefinition> types;
												try
												{
																types = assemby.MainModule.Types;
												}
												catch (Exception e)
												{
																Console.WriteLine("Loading MainModule Types failed ");
																Console.Write(e.ToString());
																Console.ReadKey();
																return null;
												}


												Console.WriteLine(string.Format("Found {0} Types", types.Count));

												return types;

								}

								public static List<string> GetOutputMemberConsole(TypeDefinition type)
								{
												List<string> members = new List<string>();


												foreach (var field in type.Fields)
												{
																Console.WriteLine($" Fields: {field.Name}");

												}
												foreach (var property in type.Properties)
												{
																Console.WriteLine($" Property: {property.Name}");

												}
												foreach (var method in type.Methods)
												{
																string name = method.Name + "(";
																bool isFirst = true;
																// Inspect parameters
																foreach (var parameter in method.Parameters)
																{
																				if (!isFirst)
																				{
																								name += ", ";
																				}

																				isFirst = false;

																				name += parameter.ParameterType.Name + " " + parameter.Name;

																}
																name += ")";
																Console.WriteLine($" Method: {name}");

												}

												return members;
								}
								public static List<string> GetOutputMember(TypeDefinition type)
								{
												List<string> member = new List<string>();


												foreach (var field in type.Fields)
												{
																member.Add($" Fields: {field.Name}");

												}
												foreach (var property in type.Properties)
												{
																member.Add($" Property: {property.Name}");

												}
												foreach (var method in type.Methods)
												{
																string name = method.Name + "(";
																bool isFirst = true;
																// Inspect parameters
																foreach (var parameter in method.Parameters)
																{
																				if (!isFirst)
																				{
																								name += ", ";
																				}

																				isFirst = false;

																				name += parameter.ParameterType.Name + " " + parameter.Name;

																}
																name += ")";
																member.Add($" Method: {name}");

												}

												return member;
								}


								public static List<string> GetCompareMember(TypeDefinition type)
								{
												if (type.FullName == "EFT.BotOwner")
												{
												}
												List<string> member = new List<string>();
												foreach (var field in type.Fields)
												{
																member.Add("f" +field.Name);

												}
												foreach (var property in type.Properties)
												{
																member.Add("p" + property.Name);

												}
												foreach (var method in type.Methods)
												{
																string name = "m" +method.Name + "(";
																bool isFirst = true;
																// Inspect parameters
																foreach (var parameter in method.Parameters)
																{
																				if (!isFirst)
																				{
																								name += ", ";
																				}

																				isFirst = false;

																				name +=  parameter.Name ;

																}

																member.Add(name + ")");
												}
												return member;
								}

								public static void WriteTableToFile(string filePath, List<string> formattedStrings)
								{
												// Tabelle in die Datei schreiben
												using (StreamWriter writer = new StreamWriter(filePath))
												{
																formattedStrings.Insert(0, string.Format("{0},{1},{2},{3},{4}", "---", "---", "-----", "---", "----------"));
																formattedStrings.Insert(0, string.Format("{0},{1},{2},{3},{4}", "Matches", "Maximum Entries", "Confidence", "SPT", "SIT"));

																List<string[]> columnValues = new List<string[]>();

																foreach (var formattedString in formattedStrings)
																{
																				try
																				{
																								string[] values = formattedString.Split(',');
																								columnValues.Add(values);
																				}
																				catch { }
																}


																int[] columnWidths = new int[columnValues[0].Length];
																for (int i = 0; i < columnValues[0].Length; i++)
																{
																				columnWidths[i] = columnValues.Max(row => row[i].Length);
																}


																foreach (var values in columnValues)
																{
																				for (int i = 0; i < values.Length; i++)
																				{
																								writer.Write(values[i].PadRight(columnWidths[i] + 1)); 
																				}
																				writer.WriteLine();
																}
												}
								}
				}
}
