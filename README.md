# DiscordWebhook
Discord webhook library in C#

# Fork Update
Add  Upload Image

```csharp
Webhook webhook = new Webhook("https://discordapp.com/api/webhooks/*/*");
var files = new List<string>();
files.Add(@"C:\Users\aiqin\Documents\image.jpg");
var data = webhook.Send("test", "ddd",null, false, null, files);
Debug.WriteLine(data.Result);
```

## Features
Currently the list of features is very limited due to the requirements that I personally had when creating this library. The goal is to send a webhook request to Discord, there is no validation and no handling of rate limits, which I will add later on. The whole purpose right now is to send quick webhook requests without having to use bloated libraries with bot features which you don't need for pure webhooks.

## References
* [Execute Webhook API](https://discordapp.com/developers/docs/resources/webhook#execute-webhook)
* [Embed Object API](https://discordapp.com/developers/docs/resources/channel#embed-object)
