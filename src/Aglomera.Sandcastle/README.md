# Documentation Instructions

To include the contents of the main README.md **(not this file!)** as the first page of the API documentation:

1. Download *"Sandcastle Help File Builder"* from https://github.com/EWSoftware/SHFB/releases

2. Add *"C:\Program Files (x86)\EWSoftware\Sandcastle Help File Builder\Extras\"* (or corresponding installation directory) to system's PATH

3. Convert/export README.md to README.html (without styles)

4. Execute *"HtmlToAml.bat"* batch file located in this directory **(will clean /docs!)**

5. Edit resulting *"Content/README.aml"* 

   *Note:* it is safe to delete everything from this directory with the exception of the .aml file

   1. Move content within:

      ```xml
      <section><!--h1-->
          <title>Project Title</title>
          <content>
              ...
          [TOC]
          </content>
      </section>
      ```

      to within the introduction:

      ```xml
      <introduction>
          ...
      </introduction>
      ```

   2. Search and remove all `<code language="none" title=" ">`

   3. Search for all `</code></code>` and replace by `</code>`

   4. Save file

6. Build the documentation using SandCastle from within VisualStudio



Copyright &copy; 2018, [Pedro Sequeira](https://github.com/pedrodbs)