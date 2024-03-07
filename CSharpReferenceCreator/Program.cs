using CSharpReferenceCreator;
using Mono.Cecil;
using System.Text.RegularExpressions;

Console.WriteLine("--- CSharpReferenceCreator ---");
Console.WriteLine("");


Mono.Collections.Generic.Collection<TypeDefinition> sptTypes = Helper.GetTypesFromFile("SPT\\Assembly-CSharp.dll");
if (sptTypes == null)
{
				Console.ReadKey();
				return;
}

Console.WriteLine("");

Mono.Collections.Generic.Collection<TypeDefinition> sitTypes = Helper.GetTypesFromFile("SIT\\Assembly-CSharp.dll");
if (sitTypes == null)
{
				Console.ReadKey();
				return;
}

Console.WriteLine("");
Console.WriteLine("");
Console.WriteLine("What would you like do do?");
Console.WriteLine("d = Dump a comparision; c = compare two classes; f = Find potentioal matches for SPT class");
string key = "";
while (key != "d" && key != "c" && key != "f")
{ 
 key = Console.ReadLine();
}

if (key == "c"){

				Console.WriteLine("Enter Classname of SPT. Leave Empty to Skip");
				var SPTClass = Console.ReadLine();

				Console.WriteLine("Enter Classname of SIT. Leave Empty to Skip");
				var SITClass = Console.ReadLine();


				if (SPTClass == null || SPTClass == "")
				{
								Console.WriteLine("SKip SPT");
				}
				else
				{
								bool found = false;
								foreach (TypeDefinition type in sptTypes)
								{
												if (type.FullName == SPTClass)
												{
																Console.WriteLine("");
																Console.WriteLine(type.FullName + "Members:");
																foreach (string info in Helper.GetOutputMemberConsole(type))
																{
																				Console.WriteLine(info);
																}
																found = true;
																break;
												}
								}
								if (!found)
								{
												Console.WriteLine("No Matching class");
								}
				}

				if (SITClass == null || SITClass == "")
				{
								Console.WriteLine("SKip SIT");
				}
				else
				{
								bool found = false;
								foreach (TypeDefinition type in sitTypes)
								{
												if (type.FullName == SITClass)
												{
																Console.WriteLine("");
																Console.WriteLine(type.FullName + "Members:");
																foreach (string info in Helper.GetOutputMemberConsole(type))
																{
																				Console.WriteLine(info);
																}
																found = true;
																break;
												}
								}
								if (!found)
								{
												Console.WriteLine("No Matching class");
								}
				}

				Console.WriteLine("");
				Console.WriteLine("");
				Console.WriteLine("Press any key to close...");
				Console.ReadKey();
				return;
} else if(key == "f"){

				List<string> dicc = new List<string>();

				Console.WriteLine("Enter Classname of SPT");
				var SPTClass = Console.ReadLine();
				while (SPTClass == null || SPTClass == "")
				{
								SPTClass = Console.ReadLine();
				}


				bool found = false;
				foreach (TypeDefinition type in sptTypes)
				{
								if (type.FullName == SPTClass)
								{
												dicc.Add("");
												dicc.Add("SPT Class:" + type.FullName + "Members:");
												foreach (string info in Helper.GetOutputMemberConsole(type))
												{
																dicc.Add(info);
												}
												found = true;
												break;
								}
				}
				if (!found)
				{
								Console.WriteLine("No Matching class");
								Console.WriteLine("");
								Console.WriteLine("");
								Console.WriteLine("Press any key to close...");
								Console.ReadKey();
				}


				Console.WriteLine("");
				Console.WriteLine("Create spt reference");
				Dictionary<string, List<string>> referenceDicc = new Dictionary<string, List<string>>();
				foreach (TypeDefinition type in sptTypes)
				{
								if (type.FullName != null)
								{
												foreach (string info in Helper.GetCompareMember(type))
												{
																if (!referenceDicc.ContainsKey(info))
																{
																				referenceDicc[info] = new List<string>();
																}
																referenceDicc[info].Add(type.FullName);

												}
								}
				}



				dicc.Add("");

				Dictionary<string, int> matching =  new Dictionary<string, int>();
				foreach (TypeDefinition type in sptTypes)
				{
								if (type.FullName == SPTClass)
								{
												Dictionary<string, int> matches =  new Dictionary<string, int>();

												List<string> methodes = Helper.GetCompareMember(type);

												foreach (string info in methodes)
												{
																if (referenceDicc.ContainsKey(info))
																{
																				List<string> lookup = referenceDicc[info];
																				if (lookup != null)
																				{
																								foreach (string item in lookup)
																								{
																												if (!matches.ContainsKey(item))
																												{
																																matches[item] = 1;
																												}
																												else
																												{
																																matches[item]++;
																												}

																								}
																				}
																}
												}
												if (matches.Count > 0)
																matching.Add(type.FullName, matches.Count);
								}
				}

				var sortedDict = matching.OrderByDescending(x => x.Value);
				matching = sortedDict.ToDictionary(x => x.Key, x => x.Value);

				foreach (var item in matching)
				{
								foreach (TypeDefinition sitType in sitTypes)
								{
												if (sitType.FullName == item.Key)
												{
																dicc.Add("");
																dicc.Add("SIT Class:" + sitType.FullName + "Members:");
																foreach (string info in Helper.GetOutputMember(sitType))
																{
																				dicc.Add(info);
																}
																found = true;
																break;
												}
								}
				}

				string filePath = "output-"+SPTClass+".txt";
				using (StreamWriter writer = new StreamWriter(filePath))
				{
								foreach (var values in dicc)
								{
												writer.WriteLine();
								}
				}

				Console.WriteLine("");
				Console.WriteLine("Result in output - " + SPTClass + ".txt");
				Console.WriteLine("");
				Console.WriteLine("");
				Console.WriteLine("Press any key to close...");
				Console.ReadKey();
				return;
}
else if (key == "d")
{

				Console.WriteLine("");
Console.WriteLine("Create spt reference");
Dictionary<string, List<string>> sptMethodsDic = new Dictionary<string, List<string>>();
foreach (TypeDefinition type in sptTypes)
{
				if (type.FullName != null)
				{
								foreach (string info in Helper.GetCompareMember(type))
								{
												if (!sptMethodsDic.ContainsKey(info))
												{
																sptMethodsDic[info] = new List<string>();
												}

												sptMethodsDic[info].Add(type.FullName);
												
								}
				}
}

Console.WriteLine("");
Console.WriteLine("Look through spt reference with sit");
List<string> sitMethodsDic = new List<string>();

foreach (TypeDefinition type in sitTypes)
{
				if (type.FullName != null)
				{
								Dictionary<string, int> matches =  new Dictionary<string, int>();

								if (type.FullName == "GClass8")
								{
												Console.WriteLine();
								}

								List<string> methodes = Helper.GetCompareMember(type);
								foreach (string info in methodes)
								{
												if (sptMethodsDic.ContainsKey(info))
												{
																List<string> lookup = sptMethodsDic[info];
																if (lookup != null)
																{
																				foreach (string item in lookup)
																				{
																								if (!matches.ContainsKey(item))
																								{
																												matches[item] = 1;
																								}
																								else
																								{
																												matches[item]++;
																								}
																								
																				}
																}
												}
								}
								if (matches.Count != 0)
								{
												var sortedDict = matches.OrderByDescending(x => x.Value);
												matches = sortedDict.ToDictionary(x => x.Key, x => x.Value);
												KeyValuePair<string, int> top = matches.First();

												sitMethodsDic.Add(string.Format("{0},{1},{2}%,{3},{4}", top.Value, methodes.Count, Math.Round((((float)top.Value / (float)methodes.Count) * 100),0), top.Key, type.FullName));
								}
								else
								{
												sitMethodsDic.Add(string.Format("{0},{1},{2},{3},{4}%", 0, methodes.Count, 0, "No Match", type.FullName));
								}
				}
}

string filePath = "output.txt";
Helper.WriteTableToFile(filePath, sitMethodsDic);

Console.WriteLine("");
Console.WriteLine("Result in output.txt");

Console.WriteLine("");
Console.WriteLine("");
Console.WriteLine("Press any key to close...");

Console.ReadKey();
return;
}