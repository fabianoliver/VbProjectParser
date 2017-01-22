using AbnfFramework;
using AbnfFramework.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VbProjectParser.Data.ABNF
{
    // p. 21
    public class ProjectProperties
    {
        public ProjectId ProjectId { get; set; }

        public IList<ProjectItem> ProjectItems { get; set; }

        public string ProjectHelpFile { get; set; }

        /// <summary>
        /// Specifies a path. MUST be ignored.
        /// </summary>
        public string ProjectExeName32 { get; set; }

        /// <summary>
        /// Specifies the short name of the VBA project.
        /// Must be between 1 - 128 characters long.
        /// </summary>
        public string ProjectName { get; set; }

        /// <summary>
        /// Specifies a Help topic identifier in ProjectHelpFile (section 2.3.1.9) associated with this VBA project.
        /// </summary>
        public int ProjectHelpId { get; set; }

        /// <summary>
        /// Specifies the description of the VBA project.
        /// Must be between 0 to 2000 characters long.
        /// </summary>
        public string ProjectDescription { get; set; }

        /// <summary>
        /// Specifies the storage format version of the VBA project. MAY be missing<4>.
        /// </summary>
        protected string _ProjectVersionCompat32 { get; set; }

        /// <summary>
        /// Specifies whether access to the VBA project was restricted by the user, the VBA host application, or the VBA project editor.
        /// Must be 22 to 28 hex digits
        /// </summary>
        public string ProjectProtectionState { get; set; }

        /// <summary>
        /// Specifies the password protection for the VBA project.
        /// A VBA project without a password MUST use 0x00 for the Data parameter for Data Encryption (section 2.4.3.2) and the Length parameter MUST be 1.
        /// A VBA project with a password SHOULD specify the password hash of the VBA project, obfuscated by Data Encryption (section 2.4.3.2). The Data parameter for Data Encryption (section 2.4.3.2) MUST be an array of bytes that specifies a Hash Data Structure (section 2.4.4.1) and the Length parameter for Data Encryption MUST be 29. The Hash Data Structure (section 2.4.4.1) specifies a hash key and 
        /// hash encoded to remove null bytes as specified by section 2.4.4.
        /// A VBA project with a password MAY<6> specify the plain text password of the VBA project, obfuscated by Data Encryption (section 2.4.3.2). In this case, the Data parameter Data Encryption (section 2.4.3.2) MUST be an array of bytes that specifies a null terminated password string encoded using MBCS using the code page specified by PROJECTCODEPAGE (section 2.3.4.2.1.4), and a Length parameter equal to the number of bytes in the password string including the terminating null character.
        /// When the data specified by <EncryptpedPassword> is a password hash, ProjectId.ProjectCLSID (section 2.3.1.2) MUST be "{00000000-0000-0000-0000-000000000000}".
        /// </summary>
        public string ProjectPassword { get; set; }

        /// <summary>
        /// Specifies whether the VBA project is visible.
        /// </summary>
        public string ProjectVisibilityState { get; set; }
 
        public ProjectProperties()
        {
            this.ProjectId = new ProjectId();
        }

        public static void Setup(ISyntax Syntax)
        {
            Syntax
                .Entity<ProjectProperties>()
                .Property(x => x.ProjectId)
                .ByRegisteredTypes(typeof(ProjectId));

            Syntax
                .Entity<ProjectProperties>()
                .EnumerableProperty(x => x.ProjectItems)
                .ByRegisteredTypes(typeof(ProjectDocModule), typeof(ProjectStdModule), typeof(ProjectClassModule), typeof(ProjectDesignerModule), typeof(ProjectPackage))
                .WithPostfix(CommonTokens.NWLN);

            Syntax
                .Entity<ProjectProperties>()
                .Property(x => x.ProjectHelpFile)
                .ByRegexPattern(CommonRegexPatterns._PATH)
                .WithPrefix(new LiteralToken("HelpFile="))
                .WithPostfix(CommonTokens.NWLN)
                .IsOptional();

            Syntax
                .Entity<ProjectProperties>()
                .Property(x => x.ProjectExeName32)
                .ByRegexPattern(CommonRegexPatterns._PATH)
                .WithPrefix(new LiteralToken("ExeName32="))
                .WithPostfix(CommonTokens.NWLN)
                .IsOptional();

            Syntax
                .Entity<ProjectProperties>()
                .Property(x => x.ProjectName)
                .ByRegexPattern(@"(" + CommonRegexPatterns._QUOTEDCHAR + "){1,128}")
                .WithPrefix(new LiteralToken("Name=") + CommonTokens.DQUOTE)
                .WithPostfix(CommonTokens.DQUOTE + CommonTokens.NWLN);

            Syntax
                .Entity<ProjectProperties>()
                .Property(x => x.ProjectHelpId)
                .ByRegexPattern(CommonRegexPatterns._INT32)
                .WithPrefix(new LiteralToken("HelpContextID=") + CommonTokens.DQUOTE)
                .WithPostfix(CommonTokens.DQUOTE + CommonTokens.NWLN);

            Syntax
                .Entity<ProjectProperties>()
                .Property(x => x.ProjectDescription)
                .ByRegexPattern(@"(" + CommonRegexPatterns._QUOTEDCHAR + "){0,2000}")
                .WithPrefix(new LiteralToken("Description=") + CommonTokens.DQUOTE)
                .WithPostfix(CommonTokens.DQUOTE + CommonTokens.NWLN)
                .IsOptional();

            Syntax
                .Entity<ProjectProperties>()
                .Property(x => x._ProjectVersionCompat32)
                .ByRegexPattern("")
                .WithPrefix(new LiteralToken("VersionCompatible32=") + CommonTokens.DQUOTE + new LiteralToken("393222000") + CommonTokens.DQUOTE + CommonTokens.NWLN)
                .IsOptional();

            // p 24
            Syntax
                .Entity<ProjectProperties>()
                .Property(x => x.ProjectProtectionState)
                .ByRegexPattern("(" + CommonRegexPatterns._HEXDIG + "){22,28}")
                .WithPrefix(new LiteralToken("CMG=") + CommonTokens.DQUOTE)
                .WithPostfix(CommonTokens.DQUOTE + CommonTokens.NWLN);

            // p 25
            Syntax
                .Entity<ProjectProperties>()
                .Property(x => x.ProjectPassword)
                .ByRegexPattern("(" + CommonRegexPatterns._HEXDIG + "){16,}")
                .WithPrefix(new LiteralToken("DPB=") + CommonTokens.DQUOTE)
                .WithPostfix(CommonTokens.DQUOTE + CommonTokens.NWLN);


            // p 25
            Syntax
                .Entity<ProjectProperties>()
                .Property(x => x.ProjectVisibilityState)
                .ByRegexPattern("(" + CommonRegexPatterns._HEXDIG + "){16,22}")
                .WithPrefix(new LiteralToken("GC=") + CommonTokens.DQUOTE)
                .WithPostfix(CommonTokens.DQUOTE + CommonTokens.NWLN);



        }
    }

   
    

    
}
