# Imani.Solutions.Core.DotNet

DotNet Core C# API solutions


## Imani.Solutions.Core.Util
### Text

Example usage code

```
 IDictionary<string, string> map = new Dictionary<string, string>();
        
   map["firstName"] = "Gregory";
            map["lastName"] = "Green";

            var actual = subject.Format("${firstName} ${lastName}", map);

            Assert.Equal("Gregory Green",actual);
```

## Packaging


nuget pack -properties Configuration=Release -Version 0.0.2
