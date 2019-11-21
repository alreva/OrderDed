# ORDER DED

This is the tool that allows ordering the dishes from Dedushka automatically.

To start, do one of the following:

## Option 1

Download the release from the releases folder (/rel) and run (requires .Net Core Runtime 3.0.1, see here: https://dotnet.microsoft.com/download/dotnet-core/3.0, most likely you will need the one for Windows 10 x64: https://dotnet.microsoft.com/download/dotnet-core/thank-you/runtime-3.0.1-windows-x64-installer):

```CMD
    OrderDed -d "your dish name" -u your.email@your.domain -p "your_password"
```

e.g.
```CMD
    OrderDed -d "Куриная грудка на гриле" -u alex.reva@gmail.dot.com -p "12345"
```

And enjoy :)

## Option 2

Clone the repository:

```CMD
    cd C:\my-git-folder-for-project
    git clone https://github.com/alreva/OrderDed.git
```

Then compile the solution (requires .Net Core SDK 3.0.101, see here: https://dotnet.microsoft.com/download/dotnet-core) and run in the same manner as in the option 1.
