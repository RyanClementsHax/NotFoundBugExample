# NotFoundBugExample

This is an example repo to show a bug with `UseExceptionHandler` with `Aspnet Core`. See the [Github issue](https://github.com/dotnet/aspnetcore/issues/31024) for more details.

## Resolution
It turns out I was using the exception handler middleware incorrectly. The exception handler middleware has an option `AllowStatusCode404Response` is set to `false` by default. The head of `master` in this repo contains the code that gets everything to work.
