# TMF Daily Watchlist Emailer

Here is my submission for The Motley Fool's emailer coding challenge! This presented an interesting
set of challenges to work through. I've made an effort to cut down on execution time as
much as possible while doing some cleanup and refactoring to make it more maintainable.
My hope is that this app will be able to keep buzzing along for another decade!

## Configuration

Three parameters need to be set in the appsettings.json file to make this run:

1. The FoolDB connection string
2. The MailChampUri base URL
3. The ArticleApi base URL

I've put the original values from the coding challenge back in there for now.

## Building & Running

I've set up this project to target .NET 5 so it will require you to have the .NET 5 SDK
downloaded. It can be built and run from Visual Studio, or, since it's a console app,
it may be easier to just do a `dotnet run` from inside the TMFDailyEmailer project folder.

## Changing User Batch Size

Since this application needs to handle sending emails for so many subscribers, it pulls in
one batch of users to process at a time. This provides advantages over attempting to load
and process users from the DB one at a time, while ensuring we don't overload our app's
memory by attempting to load all the users at once.

The size of each batch can be configured by setting UserBatchSize in appsettings.json. Right
now I've set this to 200 because that seems like a nice, round number. The MailChamp API
looks like it can handle up to 1024 at a time, which may prove to be the most efficient value
in practice. This parameter cannot be set to a value lower than 10.

## Thank you!

I'm excited to see what you think! Thank you for the opportunity and hope to talk soon.

Bill Kowalczyk
<william.kowalczyk@gmail.com>