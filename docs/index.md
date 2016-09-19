# DynamicDictionary

DynamicDictionary is a Dictionary written in C# that support multiple types, 
and where every value as potencial to be an IEnumerable of self.
It is based on DynamicDictionary implementation from [Nancy](https://github.com/NancyFx/Nancy/blob/96cde2b1ed58f25c3b21ff9d88ac931c32f692e7/src/Nancy/DynamicDictionary.cs)

# Main Features

### Support multiple types

```CSHARP
using Dynamic;

DynamicDictionary artist = new DynamicDictionary();
artist["Name"] = "Pink Floyd";
artist["Started"] = new DateTime(1965, 12, 1);
```

### It's Dynamic

```CSHARP
dynamic dyn = artist;

dyn.Genres = "Rock";

string name = dyn.name;
// name = "Pink Floyd"
```

### Every type could be IEnumerable

```CSHARP
dyn.genres.AddRange(new []{"Psycadelic Rock", "Art Rock"});

string genre = dyn.genres;
//genre = "Rock"

List<string> genres = dyn.genres;
//genres = {"Rock", "Psycadelic Rock", "Art Rock"}

string otherGenre = dyn.genres[2];
//otherGenre = "Art Rock"
```

### Custom operators support

```CSHARP
dyn.genres += "Progressive Rock";
//{"Rock", "Psycadelic Rock", "Art Rock", "Progressive Rock"}

dyn.genres -= "Art Rock";
//{"Rock", "Psycadelic Rock", "Progressive Rock"}
```

### Easily serialized
```CSHARP
var jsonString = JsonConvert.SerializeObject(dyn.ToSerializableDictionary());

//and

string serializable = JsonConvert.DeserializeObject<Dictionary<string, DynamicListValueSerializable>>(jsonString);
DynamicDictionary dictionary = DynamicDictionary.FromSerializable(serializable);

```