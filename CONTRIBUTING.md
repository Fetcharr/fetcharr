# Contributing

🎉 Hey, thanks for taking the time to contribute! 🎉

Check out some of the open issues and see if anything fits your skills. If you have an idea for a new feature, you can also open a new issue.

If that doesn't fit, you can also write documentation or fix typos, as there might be a handful.

## Code of Conduct

This project and everyone participating in it is governed by the Code of Conduct. By participating, you are expected to uphold this code. Please report unacceptable behavior to project moderators.

You can read our [Code of Conduct here](./CODE_OF_CONDUCT.md).

## Pull Requests

**Doing your first pull request?** Great, awesome to have you on-board! If you're unsure how to start, you can learn how from this *free* series: [How to Contribute to an Open Source Project on GitHub.](https://egghead.io/courses/how-to-contribute-to-an-open-source-project-on-github)

- To avoid wasted development time, **please discuss** the change you wanna make. This can be done with on [GitLab Issues](https://git.maxtrier.dk/natamo/paracord/-/issues) or [GitHub Issues](https://github.com/maxnatamo/paracord/issues). If possible, discuss it publicly, so other people can chime in.
- The `develop` branch is used for the current development build. For that reason, **please, do not submit your PRs against the `main` branch.**
- Ensure that your code **respects the repository's formatting standard** (defined [here](/.editorconfig)). To do this, you can run:
```bash
dotnet format --verify-no-changes
```
Or, with [Nuke](https://nuke.build):
```bash
nuke Format
```
- Make sure your code **passes the tests**. Do do this, you can run:
```bash
dotnet test
```
Or, with [Nuke](https://nuke.build):
```bash
nuke Test
```

It is also recommended to add new tests, if you're implementing a new feature.

## Development Setup

There are a few ways of setting up the project for development:
- [Installing all tools locally](#required-tools)
- [or by using single-click setups](#single-click-setup)

### Developing locally

#### Required Tools
- Code editor
  - We recommend [VS Code](https://code.visualstudio.com/). Upon opening the project, a few extensions will be automatically recommended for install.
- [.NET SDK 8.0](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)
- [Git](https://git-scm.com/downloads)

#### Getting started

1. Fork the repository to your own account and clone it:
    ```sh
    git clone https://github.com/YOUR_USERNAME/fetcharr.git
    cd fetcharr/
    ```

2. Add the `upstream` remote:
    ```sh
    git remote add upstream https://github.com/fetcharr/fetcharr.git
    ```

3. Create a new branch:
    ```sh
    git checkout -b BRANCH_NAME develop
    ```

  - For the sake of the maintainers, we recommend you give your branch a meaningful name which sumarizes what the contribution does.
    - Good examples:
        - `feat-healthz-endpoint`
        - `fix-plex-cache-invalidation`
        - `docs-readme-typo`
    - Bad examples:
        - `bug-fix`
        - `feature`
        - `develop`

4. Setup your development environment:
```sh
dotnet build src/Fetcharr.sln
dotnet run --project src/API/src/Fetcharr.API.csproj
```

5. Add your contribution and test your changes.
- Be sure to follow both the [code guidelines](#code-guidelines) and [commit guidelines](#commit-guidelines).
- If you need to update your fork, rebase from `develop`:
    ```sh
    git fetch upstream
    git rebase upstream/develop
    git push origin BRANCH_NAME -f
    ```

### Single-click setup

If you prefer not installing anything locally, you can open the project in [Github Codespaces](https://github.com/features/codespaces) or [Gitpod](https://gitpod.io) by clicking the buttons below:

<div style="display: flex; align-items: center" align="center">
  <a href="https://gitpod.io/#https://github.com/fetcharr/fetcharr/tree/develop">
    <img src="https://gitpod.io/button/open-in-gitpod.svg" />
  </a>
  <a href="https://codespaces.new/Fetcharr/fetcharr/tree/develop?quickstart=1">
    <img src="https://github.com/codespaces/badge.svg" />
  </a>
</div>

## Code Guidelines

- Your code **must** be formatted correctly; otherwise the tests will fail.
  - Before committing, it is a good idea to run `dotnet format` to make sure everything is correct.
  - If you use VS Code, it should also warn you about incorrectly formatted code (although, not always).
- Always rebase your commit to the latest `develop` branch. **Do not merge** `develop` into your branch.
- All commits **must** follow the guidelines in [Commit Guidelines](#commit-guidelines).

## Commit Guidelines

This repository takes use of a slightly modified version of the [Angular commit guidelines](https://github.com/angular/angular/blob/main/CONTRIBUTING.md#-commit-message-format).

### Types

| Types    | Description                                                                                              |
| -------- | -------------------------------------------------------------------------------------------------------- |
| build    | New build version.                                                                                       |
| chore    | Changes to the build process or auxiliary tools such as changelog generation. No production code change. |
| ci       | Changes related to continuous integration only (GitHub Actions, CircleCI, etc.).                         |
| docs     | Documentation only changes.                                                                              |
| feat     | A new feature.                                                                                           |
| fix      | A bug fix, whether it fixes an existing issue or not.                                                    |
| perf     | A code change that improves performance.                                                                 |
| refactor | A code change that neither fixes a bug nor adds a feature.                                               |
| style    | Changes that do not affect the meaning of the code (white-space, formatting, missing semi-colons, etc.). |
| test     | Adding missing or correcting existing tests.                                                             |

### Scopes

Instead of using a pre-defined list of scopes, the scope should define the affected component in the project tree.

For example, if you add a new feature to the Sonarr provider, the scope might be `sonarr`. Other examples include `plex`, `cache`, `config`, etc.

Please, try to be precise enough to describe the field of the change, but not so precise that the scope loses it's meaning.

## Versioning

This repository takes use of [Semantic Versioning](https://semver.org) for new releases.
