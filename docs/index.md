# Nexus Git
Nexus Git provides access to Git repositories in Roblox Studio.
It uses existing project structures (like Rojo) to allow developers
who don't use external editors to contribute to Git-based repositories.

# Project Suspended
**Development of Nexus Git by the repository owner has been stopped due
to the estimated time required to make Nexus Git robust enough to keep
using. Bug fixes are still supported but feature requests will not
be implemented. Pull requests are still accepted. Additionally,
two-way syncing is an option in Rojo 0.6.**

## Currently Supported Project Structures
Rojo 0.4 and Rojo 0.5 are supported with this plugin for syncing into Roblox
Studio and syncing out of Roblox Studio. Additional projects can be supported,
but Rojo should be considered for new projects due to the robustness of the tool
and the support it has given from several developers.

## Utility for Rojo Users
If you are an existing Rojo user, Nexus Git can still
be useful since syncing is two-way instead of one-way.
[Rojo two-way syncing is planned](https://github.com/rojo-rbx/rojo/issues/164),
but is not out yet. While not completely useful for scripts,
exporting models is time consuming with stock Rojo. Additionally,
Nexus Git supports both Rojo 0.4 and Rojo 0.5, which can be useful
if you need to quickly import a Rojo 0.4 project into Roblox Studio,
or export either Rojo 0.4 or Rojo 0.5.

### Limitation: No RBXM Support
RBXM are Roblox model files that are stored as binary
data. Support for these files is not planned unless an
external library written for .NET Standard or .NET Core
is made and maintained similar to [rbx-dom in Rust](https://github.com/rojo-rbx/rbx-dom).

## Warning: Alpha Release
Nexus Git is current in an Alpha state. The overview of
known limitations includes that will be resolved:
- Git Branches aren't supported ([Trello card](https://trello.com/c/UWRzTdn9/35-add-branches-view))<br>
- Git Submodules aren't supported ([Trello card](https://trello.com/c/tP433r0v/42-add-submodules-support))<br>
- The client-side API dump is not up to date ([Trello card](https://trello.com/c/GLjr1lmk/53-add-updating-api-dump))<br>
- RMXMX is current not supported ([Trello card](https://trello.com/c/nC0IwA9l/50-add-rbxmx-support))<br>

These will be addressed in version 0.2.0. Due to blockers
that prevent it from being updated, it will be several
months before the next feature release. Major bugs will
be fixed as minor releases if they come up.