using System;
using System.IO;

class Prog {
    static string home = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
    static string notesPath = Path.Combine(home, ".notes");
	static ConsoleColor nameColour = ConsoleColor.Red;
	static ConsoleColor contColour = ConsoleColor.Blue;
	static ConsoleColor dateColour = ConsoleColor.Yellow;
	static string showDate = "verbose";
	static void Main(string[] args) {
		Prog p = new Prog();
		bool program = true;
		p.loadConf();
		for (int i = 0; i < args.Length; i++) {
            var arg = args[i];
            switch (arg) {
				case "-l":
					p.showNotes();
					program = false;
					break;
				case "-d":
					if (i+1 >= args.Length) { Console.WriteLine("Error. No note name provided."); }
					else {
						string? fileName = args[i+1];
						if (File.Exists(Path.Combine(notesPath, fileName))) {
							File.Delete(Path.Combine(notesPath,fileName));
							Console.WriteLine("Deleted " + fileName);
						}
						else { Console.WriteLine("No such note."); }
					}
					program = false;
					break;
				case "-help":
				case "-h":
		    		Console.WriteLine("Run with no arguments to open app.\n-d <noteName> will delete the note.\n-e <noteName> will give you a prompt to change the contents of the note you named.\n-l prints all notes without opening app.\n-a <noteName> \"<noteContents>\" to add a note without opening the CLI.\n-help writes this message.");
			    	program = false;
                    break;
                case "-e":
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
				  break;
                case "-a":
				case "-add":
		    		if (i+2 >= args.Length) { Console.WriteLine("Error. No note name and/or content provided"); program = false; }
		    		else {
		    			string? fileName = args[i+1];
			    		string? fileCont = string.Join(" ", args.Skip(i+2));
			    		p.WriteFile(fileCont, notesPath, fileName);
				    	p.showNotes();
					program = false;
				    }
                    break;
			}
        }
		if (program) { p.Start(); }
	}
	public void Start() {
		while(true) {
            showNotes();
			Console.Write("\n\nMake new Note?\nY/N: ");
			string? i = Console.ReadLine();
			if (i.Contains("y") || i.Contains("Y")) { MakeNote(); }
			else { break; }
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
		    if (File.Exists(Path.Combine(notesPath, filename))) {
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
			Console.ForegroundColor = nameColour;
			Console.WriteLine(Path.GetFileName(f) + ":");
			Console.ForegroundColor = contColour;
			Console.WriteLine("  " + File.ReadAllText(f));
			if (showDate != "false") {
				Console.ForegroundColor = dateColour;
				FileInfo FInfo = new FileInfo(f);
				if (showDate == "verbose") { Console.WriteLine("  Created: " + FInfo.CreationTime); }
				else if (showDate == "short") { Console.WriteLine("  Created: " + FInfo.CreationTime.ToShortDateString()); }
				else if (showDate == "long") { Console.WriteLine("  Created: " + FInfo.CreationTime.ToLongDateString()); }
			}
			Console.ResetColor();
		}
	}

	public void loadConf() {
        string confDir = ".config/krillNotes";
		string confName = "noteConf";
		string confFile = Path.Combine(home, confDir, confName);
		if (File.Exists(confFile)) {
			foreach (string line in File.ReadLines(confFile)) {
				if (line.Contains("=")) {
					string[] parts = line.Split('=', 2);
					string key = parts[0].Trim();
					string value = parts[1].Trim();
					if (Enum.TryParse<ConsoleColor>(value, out ConsoleColor parsedColor)) {
						if (key == "nameColour") { nameColour = parsedColor; }
						if (key == "contColour") { contColour = parsedColor; }
						if (key == "dateColour") { dateColour = parsedColor; }
					}
                    if (key == "showDate") { showDate = value; }
					if (key == "notesPath") { notesPath = Path.Combine(home, value); }
				}
			}
		}
		else {
            Directory.CreateDirectory(Path.Combine(home, confDir));
			File.WriteAllText(confFile, "nameColour=Red\ncontColour=Blue\nshowDate=false\ndateColour=Magenta\nnotesPath=.notes");
		}
	}
}

