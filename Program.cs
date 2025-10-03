using System;
using System.IO;

class Prog {
    static string home = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
    static string notesPath = home + "/.notes";
	static void Main(string[] args) {
		Prog p = new Prog();
		bool program = true;
		for (int i = 0; i < args.Length; i++) {
			if (args[i] == "-l") {
				p.showNotes();
				program = false;
			}
			if (args[i] == "-d") {
				if (i+1 >= args.Length) { Console.WriteLine("Error. No note name provided."); }
				else { 
					string? fileName = args[i+1]; 
					if (File.Exists(Path.Combine(notesPath, fileName))) {
						File.Delete(Path.Combine(notesPath, fileName));
						Console.WriteLine(fileName + " deleted.");
					}
					else { Console.WriteLine("doesnt exist."); }
				}
				program = false;
			}
			if (args[i] == "-help" || args[i] == "--help" || args[i] == "-h") {
				Console.WriteLine("Run with no arguments to open app.\n-d <noteName> will delete the note.\n-e <noteName> will give you a prompt to change the contents of the note you named.\n-help writes this message.");
				program = false;
			}
			if (args[i] == "-e") {
                if (i+1 >= args.Length) { Console.WriteLine("Error. No note name provided."); }
                else {
					string? fileName = args[i+1];
					if (File.Exists(Path.Combine(notesPath, fileName))) {
			    		Console.Write("\nNew Content\n> ");
					    string? newCont = Console.ReadLine();
			   	 		p.WriteFile(newCont, notesPath, fileName, true);
						p.showNotes();
						program = false;
					}
					else { Console.WriteLine(fileName + " doesnt exist."); }
                }
			}
		}
		if (program) {
			p.Start();
		}
	}
	public void Start() {
		while(true) {
            showNotes();
			Console.Write("\n\nMake new Note?\nY/N: ");
			string? i = Console.ReadLine();
			if (i.Contains("y") || i.Contains("Y")) {
				MakeNote();
			}
			else {
				break;
			}
		}
	}

	public void MakeNote() {
		while(true) {
            bool exit = false;
			Console.Write("Please type name of note.\n> ");
			string? name = Console.ReadLine();
            while(true) {
	    		if (!string.IsNullOrWhiteSpace(name)) {
		    		Console.Clear();
		    		Console.Write($"{name}\n> ");
		    		string? cont = Console.ReadLine();
	    			WriteFile(cont, notesPath, name);
		    		Console.WriteLine(Directory.GetFiles(notesPath));
                    exit = true;
		    		break;
		    	}
		    	else {
		    		Console.Clear();
		    		Console.WriteLine("Error. Filename cant be null.");
		    	}
            }
            if (exit) { break; }
		}
	}

	public void WriteFile(string? con, string? loc, string? filename, bool ignoreOverwrite = false) {
		string? path = Path.Combine(loc, filename);
        if (!ignoreOverwrite) {
		    if (File.Exists(notesPath + "/" + filename)) {
	    		Console.Write("\nError. File exists. Overwrite?\nY/N: ");
	    		string? yn = Console.ReadLine();
		    	if (yn.Contains("Y") || yn.Contains("y")) {
	    			File.WriteAllText(path,con);
	    		}
    		}
	    	else {
		    	File.WriteAllText(path, con);
	    	}
        }
	    else {
		    File.WriteAllText(path, con);
	    }
	}
	public void showNotes() {
		if (!Directory.Exists(notesPath)) {
				Directory.CreateDirectory(notesPath);
		}
        Console.Clear();
		string? [] files = Directory.GetFiles(notesPath);
		Console.WriteLine("Notes:");
		foreach (var f in files) {
			Console.ForegroundColor = ConsoleColor.Red;
			Console.WriteLine(Path.GetFileName(f) + ":");
			Console.ForegroundColor = ConsoleColor.Blue;
			Console.WriteLine("  " + File.ReadAllText(f));
			Console.ResetColor();
		}
	}
}
