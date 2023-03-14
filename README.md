# CH.MailMinder

`CH.MailMinder` is a worker service that uses Microsoft Graph to retrieve any unread emails at the end of the day, and sends a Teams message reminding you of any unread emails.

## Configuration

To use the `CH.MailMinder` service, you'll need to configure the following `appsettings`:

```json
"MailMinder": {
  "EODHour24": 18,
  "Graph": {
    "ClientId": "",
    "TenantId": ""
  },
  "DevModeEnabled": true
}
```

* EODHour24: The hour (24h) which will indicate the end of day, since each person might have a different EOD.
* Graph.ClientId: The client id of the app registration.
* Graph.TenantId: The tenant id of the app registration.
* DevModeEnabled: When true, this will always trigger the EOD notification.

You'll need to configure these settings in your application's appsettings.json file or in another configuration provider that the application uses. You'll also need to register an app in Azure AD and configure it with the appropriate permissions to access Microsoft Graph.

Once you've configured your app and registered it in Azure AD, you can build and run the CH.MailMinder service to start receiving notifications about unread emails at the end of each day.
