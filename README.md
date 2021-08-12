# SixDegrees

A website to calculate someone's Bacon Number from IMDB


## Re-building templates

If you ever need to re-build the templates (such as upgrading between .net 
previews), You can use `dotnet new blazorwasm --hosted true --auth Individual --use-local-db true`, which will generate a new project.

## Dealing with UTF8 BOM

Some of the .Net generated template files have a pesky UTF* BOM at the start of the file. To find these, you can use `find . -type f -exec grep -Hl "^$(printf '\xef\xbb\xbf')" {} \;`. To remove them, use `find . -type f -exec sed -i  "s/^$(printf '\xef\xbb\xbf')//" {} \;`, but make sure you don't touch the .git directory...