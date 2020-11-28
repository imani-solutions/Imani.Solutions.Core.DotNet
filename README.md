# Imani.Solutions.Core.DotNet

DotNet Core C# API solutions

## Imani.Solutions.Core.Util

### ConfigSettings

This object supports geting properties for environment
variables or input arguments (prefix with --PROPERTY_NAME)


Create a new instance.

```c#
ConfigSettings  config = new ConfigSettings();
```

Setting property with an environment variable (ex: KAFKA_BOOTSTRAP_SERVERS)

```shell
# Example for UNIT/LINUX
export KAFKA_BOOTSTRAP_SERVERS=localhost
```
Or setting property with an input argument

```shell
dotnet run --KAFKA_BOOTSTRAP_SERVERS=localhost
```

Get a string property by name (throws an argument exception if the property does not exist)

```c#
string servers = config.GetProperty("KAFKA_BOOTSTRAP_SERVERS");
```


Use a default value if the property does not exist.

```c#
string servers = config.GetProperty("KAFKA_BOOTSTRAP_SERVERS","localhost");
```

Get an integer property

```c#
int port = config.GetPropertyInteger("KAFKA_PORT");
```

Use a default integer if the property not set.

```c#
int port = config.GetPropertyInteger("KAFKA_PORT",9092);
```


### Text

Example usage code

```c#
 Text text = new Text();
 IDictionary<string, string> map = new Dictionary<string, string>();

 map["firstName"] = "Gregory";
 map["lastName"] = "Green";

 var actual = textt.Format("${firstName} ${lastName}", map);

 Assert.Equal("Gregory Green",actual);
```

## Packaging

dotnet pack -c Release

nuget pack -properties Configuration=Release -Version 0.0.2
