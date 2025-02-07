# 017 Task Based Async Pattern

## Lecture

[![# One To One Relationship](https://img.youtube.com/vi/hNoaqRD51Mo/0.jpg)](https://www.youtube.com/watch?v=hNoaqRD51Mo)

## Instructions

In `HomeEnergyApi/Models/HomeUsageData.cs`
- Create a public class `HomeUsageData`
    - Give `HomeUsageData` the following public properties / types
        - Id / int
        - Monthly Electric Usage / int
        - HasSolar / bool
        - HomeId / int
        - Home / Home?
            - Add the `JsonIgnore` attribute to Home.
            - Add `= null!` to make Home non-nullable. (see 'null-forgiving' link in Resources)
    - Ensure all properties have getters/setters

In `HomeEnergyApi/Models/HomeModel.cs`
- On `Home` create a public property `HomeUsageData` of type `HomeUsageData?`
    - Ensure this property has getter/setter

In `HomeEnergyApi/Models/HomeDbContext.cs`
- On `HomeDbContext` create a public property `HomeUsageDatas` of type `DbSet<HomeUsageData>`

In `HomeEnergyApi/Models/HomeRepository.cs`
- Modify `HomeRepository.Save()` so that...
    - If `home.HomeUsageData` is null,
    - A `HomeUsageData` is added to `context.HomeUsageDatas`,
    - With it's `Home` property set to `home`

In your terminal
- ONLY IF you are working on codespaces or a different computer/environment as the previous lesson and don't have `dotnet-ef` installed globally, run `dotnet tool install --global dotnet-ef`, otherwise skip this step
    - To check if you have `dotnet-ef` installed, run `dotnet-ef --version`
- Run `dotnet ef migrations add AddHomeUsageDataTable`
- Run `dotnet ef database update`
    
## Additional Information
- Along with `using` statements being added, any packages needed for the assignment have been pre-installed for you, however in the future you may need to add these yourself.

## Building toward CSTA Standards:
- Decompose problems into smaller components through systematic analysis, using constructs such as procedures, modules, and/or objects (3A-AP-17) https://www.csteachers.org/page/standards
- Create artifacts by using procedures within a program, combinations of data and procedures, or independent but interrelated programs (3A-AP-18) https://www.csteachers.org/page/standards
- Compare and contrast fundamental data structures and their uses (3B-AP-12) https://www.csteachers.org/page/standards
- Construct solutions to problems using student-created components, such as procedures, modules and/or objects (3B-AP-14) https://www.csteachers.org/page/standards
- Demonstrate code reuse by creating programming solutions using libraries and APIs (3B-AP-16) https://www.csteachers.org/page/standards
- Modify an existing program to add additional functionality and discuss intended and unintended implications (e.g., breaking other functionality) (3B-AP-22) https://www.csteachers.org/page/standards

## Resources
- https://learn.microsoft.com/en-us/ef/core/modeling/relationships/one-to-one
- https://learn.microsoft.com/en-us/dotnet/standard/serialization/system-text-json/ignore-properties
- https://en.wikipedia.org/wiki/Serialization
- https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/operators/null-forgiving

Copyright &copy; 2025 Knight Moves. All Rights Reserved.
