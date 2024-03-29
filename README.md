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

Get a boolean value

```c#
bool actual = new ConfigSettings().GetPropertyBoolean("BOOL_PROP");
```

Get a decrypted secret

```c#
string secret = this.subject.GetPropertySecret("MYSECRET");
```

Get a decrypted password secret
```c#
char[] password = this.subject.GetPropertyPassword("MYPASSWORD");
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

# Imani.Solutions.Core.API.NET

## Http


### GET

```c#
string url = "http://www.TheRevelationSquad.com";

var subject =new Http();
var httpResponse = subject.Get(url);
Assert.IsNotNull(httpResponse);
Assert.AreEqual(200,httpResponse.StatusCode);
Console.WriteLine("HTML:"+httpResponse.Body);
```

### GET

```c#
string url = "http://www.TheRevelationSquad.com";

var subject =new Http();
var httpResponse = subject.Get(url);
Assert.IsNotNull(httpResponse);
Assert.AreEqual(200,httpResponse.StatusCode);
Console.WriteLine("HTML:"+httpResponse.Body);
```

### POST

```c#
string url = "http://www.TheRevelationSquad.com";


string payload = "{}";
string contentType = "application/json";
var httpResponse = subject.Post(url,payload,contentType);
Assert.IsNotNull(httpResponse);

Assert.AreEqual(200,httpResponse.StatusCode);
Console.WriteLine("HTML:"+httpResponse.Body);
```

# Imani.Solutions.Core.API.Serialization

## JsonSerde

### Serialization

```c#
var subject = new JsonSerde<DomainQa>();
DomainQa expected = new DomainQa(){
    Id = "hello"
};
           
var actualSerialized = subject.Serialize(expected);
Assert.IsNotNull(actualSerialized);

var actual = subject.Deserialize(actualSerialized);
Assert.AreEqual(expected.Id,actual.Id);
```

## Deserialization

```c#
var subject = new JsonSerde<DomainQa>();
 
string json ="{\"Id\" : \"You\"}";
DomainQa actual = subject.Deserialize(json);
Assert.AreEqual("You",actual.Id);

```


# Packaging

*Pre steps*

- Update Imani.Solutions.Core.nuspec version
- Update Imani.Solutions.Core.csproj



```shell script
dotnet build -c Release
```

```shell script
dotnet pack -c Release
```

```shell script
nuget pack Imani.Solutions.Core.csproj -properties Configuration=Release -Version 0.1.0
```

Upload file  Imani.Solutions.Core.<version>.nupkg to NuGet
