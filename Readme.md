# DynamicDictionary


### What is it?
It's a dictionary based on Nancy.DynamicDictionary but every member is a List and case insensitive by default

### How does it work?

```csharp

using Dynamic;

dynamic artist = new DynamicDictionary();

string mainName = "Pink Floyd";
List<string> aliases = new List<string>
    {
        "The Pink Floyd",
        "Floyd",
        "핑크 플로이드",
    };

artist.name = mainName;
artist.name.AddRange(aliases);

string Name = artist.name;
//The Pink Floyd

List<string> Names = artist.name;
//{"Pink Floyd", "The Pink Floyd", "Floyd", "핑크 플로이드"}

artist.name -= "핑크 플로이드";
//{"Pink Floyd", "The Pink Floyd", "Floyd"}

artist.name += "New Name";
//{"Pink Floyd", "The Pink Floyd", "Floyd", "New Name"}

artist.name.Value = "Make This main name";
//{"Make This main name", "Pink Floyd", "The Pink Floyd", "Floyd", "New Name"}

string newMainName = artist.Name;
"Make This main name"

````