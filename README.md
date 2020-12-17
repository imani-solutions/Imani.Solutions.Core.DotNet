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

#### Passwords

You must set the salt environment variable CRYPTION_KEY.

```shell script
# example
export CRYPTION_KEY=xQwdSd23sdsd23
```
Generate encrypted password
```c#
var encrypted= config.EncryptPassword(expected);
```

Set encrypted password in the environment variable
(along with salt key).

```shell script
# example
export CRYPTION_KEY=xQwdSd23sdsd23
export MYPASSWORD=outputFrom[config.EncryptPassword(expected)]
```

Get encrypted password by environment variable named MYPASSWORD
```c#
char [] password = config.GetPropertyPassword("MYPASSWORD");
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

### Cryption

```c#
Cryption subject = new Cryption(key);
string actual = subject.EncryptText(expected);
Assert.AreEqual(expected, subject.DecryptText(actual));
```


## Packaging

dotnet pack -c Release

nuget pack -properties Configuration=Release -Version 0.0.4
