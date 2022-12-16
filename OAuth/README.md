# Introduction 
This is a demo application on how to make a connection to IoL via OAuth.

The code that performs the connection is in `Logtrade.Iol.Examples.Core` while the `Logtrade.Iol.Examples.OAuth.Web` project shows it in action.

[Connection generator and response code](https://github.com/logtrade-technology-ab/data-sharing-examples/blob/main/OAuth/Logtrade.Iol.Examples.OAuth.Core/Services/OAuthConnector.cs)

The first time running the web project will give you instructions on how to set up an application in your IoL account in order to test the process.

You will need to use ngrok as a reverse proxy for your localhost, or something similar, as to successfully complete an OAuth connection IoL must post a confirmantion to your server and expects to recieve a response.

You can find further documentation about how to do this in the [developer documentation](https://developer.logtrade.info/logtrade-iol-core-programmers-guide/connecting-applications/).
