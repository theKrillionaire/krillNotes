Krillionaire's Notes App.

Simple CLI notes app.

Commands:
  Run with no arguments to open app.  
  `-d <noteName>` will delete the note.  
  `-e <noteName>` will give you a prompt to change the contents of the note you named.  
  `-l` prints all notes without opening app.  
  `-a <noteName> "<noteContents>"` creates a new note called <noteName> containing <noteContents> (make sure you wrap the second one in "'s)  
  `-help` writes this message.  

Configuration:
  Config is saved to `/home/<username>/.config/krillNotes/noteConf` or `C:/Users/<username>/.config/krillNotes/noteConf`.
  `nameColour=Red' is the default note name colour.
  `contColour=Blue' is the default note content colour.
  `notesPath=.notes' is the default note path. change this to a folder name (will save to `/home/<username>/<notesPath>` or `C:/Users/<username>/<notesPath`.

Colors:
  Black, DarkBlue, DarkGreen, DarkCyan, DarkRed, DarkMagenta, DarkYellow, Gray, DarkGray, Blue, Green, Cyan, Red, Magenta, Yellow, White 

Install:
  to install, just go to Releases, get the latest release, extract it, and then run it.
