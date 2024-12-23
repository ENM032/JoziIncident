USE [JoziIncident]
GO
ALTER TABLE [dbo].[ServiceRequests] DROP CONSTRAINT [FK_dbo.ServiceRequests_dbo.Users_UserId]
GO
ALTER TABLE [dbo].[Incidents] DROP CONSTRAINT [FK_dbo.Incidents_dbo.Users_UserId]
GO
/****** Object:  Index [IX_UserId]    Script Date: 11/18/2024 7:29:23 PM ******/
DROP INDEX [IX_UserId] ON [dbo].[ServiceRequests]
GO
/****** Object:  Index [IX_UserId]    Script Date: 11/18/2024 7:29:23 PM ******/
DROP INDEX [IX_UserId] ON [dbo].[Incidents]
GO
/****** Object:  Table [dbo].[Users]    Script Date: 11/18/2024 7:29:23 PM ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Users]') AND type in (N'U'))
DROP TABLE [dbo].[Users]
GO
/****** Object:  Table [dbo].[ServiceRequests]    Script Date: 11/18/2024 7:29:23 PM ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ServiceRequests]') AND type in (N'U'))
DROP TABLE [dbo].[ServiceRequests]
GO
/****** Object:  Table [dbo].[LocalEvents]    Script Date: 11/18/2024 7:29:23 PM ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[LocalEvents]') AND type in (N'U'))
DROP TABLE [dbo].[LocalEvents]
GO
/****** Object:  Table [dbo].[Incidents]    Script Date: 11/18/2024 7:29:23 PM ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Incidents]') AND type in (N'U'))
DROP TABLE [dbo].[Incidents]
GO
/****** Object:  Table [dbo].[__MigrationHistory]    Script Date: 11/18/2024 7:29:23 PM ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[__MigrationHistory]') AND type in (N'U'))
DROP TABLE [dbo].[__MigrationHistory]
GO
USE [master]
GO
/****** Object:  Database [JoziIncident]    Script Date: 11/18/2024 7:29:23 PM ******/
DROP DATABASE [JoziIncident]
GO
/****** Object:  Database [JoziIncident]    Script Date: 11/18/2024 7:29:23 PM ******/
CREATE DATABASE [JoziIncident]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'JoziIncident', FILENAME = N'C:\Users\Zozz\JoziIncident.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'JoziIncident_log', FILENAME = N'C:\Users\Zozz\JoziIncident_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
 WITH CATALOG_COLLATION = DATABASE_DEFAULT
GO
ALTER DATABASE [JoziIncident] SET COMPATIBILITY_LEVEL = 150
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [JoziIncident].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [JoziIncident] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [JoziIncident] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [JoziIncident] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [JoziIncident] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [JoziIncident] SET ARITHABORT OFF 
GO
ALTER DATABASE [JoziIncident] SET AUTO_CLOSE ON 
GO
ALTER DATABASE [JoziIncident] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [JoziIncident] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [JoziIncident] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [JoziIncident] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [JoziIncident] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [JoziIncident] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [JoziIncident] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [JoziIncident] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [JoziIncident] SET  ENABLE_BROKER 
GO
ALTER DATABASE [JoziIncident] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [JoziIncident] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [JoziIncident] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [JoziIncident] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [JoziIncident] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [JoziIncident] SET READ_COMMITTED_SNAPSHOT ON 
GO
ALTER DATABASE [JoziIncident] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [JoziIncident] SET RECOVERY SIMPLE 
GO
ALTER DATABASE [JoziIncident] SET  MULTI_USER 
GO
ALTER DATABASE [JoziIncident] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [JoziIncident] SET DB_CHAINING OFF 
GO
ALTER DATABASE [JoziIncident] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [JoziIncident] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [JoziIncident] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [JoziIncident] SET ACCELERATED_DATABASE_RECOVERY = OFF  
GO
ALTER DATABASE [JoziIncident] SET QUERY_STORE = OFF
GO
USE [JoziIncident]
GO
/****** Object:  Table [dbo].[__MigrationHistory]    Script Date: 11/18/2024 7:29:23 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[__MigrationHistory](
	[MigrationId] [nvarchar](150) NOT NULL,
	[ContextKey] [nvarchar](300) NOT NULL,
	[Model] [varbinary](max) NOT NULL,
	[ProductVersion] [nvarchar](32) NOT NULL,
 CONSTRAINT [PK_dbo.__MigrationHistory] PRIMARY KEY CLUSTERED 
(
	[MigrationId] ASC,
	[ContextKey] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Incidents]    Script Date: 11/18/2024 7:29:23 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Incidents](
	[Id] [nvarchar](128) NOT NULL,
	[Location] [nvarchar](max) NULL,
	[Category] [nvarchar](max) NULL,
	[Description] [nvarchar](max) NULL,
	[MediaFilePath] [nvarchar](max) NULL,
	[UserId] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_dbo.Incidents] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[LocalEvents]    Script Date: 11/18/2024 7:29:23 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[LocalEvents](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Title] [nvarchar](max) NULL,
	[Description] [nvarchar](max) NULL,
	[Date] [datetime] NOT NULL,
	[ImageUrl] [nvarchar](max) NULL,
	[Category] [nvarchar](max) NULL,
 CONSTRAINT [PK_dbo.LocalEvents] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ServiceRequests]    Script Date: 11/18/2024 7:29:23 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ServiceRequests](
	[RequestID] [nvarchar](128) NOT NULL,
	[Description] [nvarchar](max) NULL,
	[Status] [nvarchar](max) NULL,
	[Progress] [float] NOT NULL,
	[IsIndeterminate] [bit] NOT NULL,
	[UserId] [uniqueidentifier] NOT NULL,
	[SearchRequestType] [nvarchar](max) NULL,
 CONSTRAINT [PK_dbo.ServiceRequests] PRIMARY KEY CLUSTERED 
(
	[RequestID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Users]    Script Date: 11/18/2024 7:29:23 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Users](
	[Id] [uniqueidentifier] NOT NULL,
	[Username] [nvarchar](50) NOT NULL,
	[Email] [nvarchar](max) NOT NULL,
	[Password] [nvarchar](max) NOT NULL,
	[DateCreated] [datetime] NOT NULL,
	[ProfileImgPath] [nvarchar](max) NULL,
	[Role] [nvarchar](max) NOT NULL,
 CONSTRAINT [PK_dbo.Users] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
INSERT [dbo].[__MigrationHistory] ([MigrationId], [ContextKey], [Model], [ProductVersion]) VALUES (N'202411111055402_InitialCreate', N'ST10091324_PROG7312_Part1.Migrations.Configuration', 0x1F8B0800000000000400DD5CDB6EDC36107D2FD07F10F4D416AEE5B55BB435D609925D3B58348E0DAF13F4CDA025EE9AA8446D29CAB551F4CBFAD04FEA2F74A82B4552B7BD59090218162F874372E670C899F8BF7FFE1DBF7E0A7CEB11B38884F4CC1E1D1ED916A66EE811BA3CB363BEF8FE67FBF5ABAFBF1A9F7BC193F5296F7722DA414F1A9DD90F9CAF4E1D27721F7080A2C380B82C8CC2053F74C3C0415EE81C1F1DFDE28C460E06081BB02C6B7C13534E029C7CC0E724A42E5EF118F997A187FD282B879A79826A7D40018E56C8C567F6FC760478A393E31FEEAE6FAEDEFD74323ABEBB468C8F0E93BEB6F5C62708C49A637F615B88D290230E429F7E8CF09CB3902EE72B2840FEEDF30A43BB05F2239C4DE6B46CDE755E47C7625E4ED9318772E38887414FC0D149B6508EDA7DADE5B68B8584A53C8725E7CF62D6C9729ED933EA120F536E5BEA60A7139F8986ADAB7D98631C58B52D0F0AB501ED12FF0EAC49ECF398E1338A63CE907F605DC7F73E717FC5CFB7E1EF989ED1D8F765D9417AA8AB1440D1350B5798F1E71BBCC867E4D99653EDE7A81D8B6E529F6CAE9C81D6DBD6257A7A8FE9923F803D1CFF6C5B17E4097B7949A62E1F290123814E9CC5F0F9010446F73E2EEA9DC631DF876EB6A9B523C3AF9D466E1E6882385E86EC79E7034D71E432B2DACBA42EB147D005F1F135E20F3B1F0D6883953AF22E265EFB767F408F6499ECB001CCB66EB09F54460F649512D5A1A8B8CB8D2902B15918DC847ED6A5ACB9BB456C89C1646F4363F53C8C99AB0834764AD36F248454BAF5C940F4FF1288A0DB261BB696C26F0D0AF9E3D116F4F13C40C4DF82D6F79CDE358AA23F43D644963B1A790A1C3661187E16838BA25B12AC310B162E803866C1722FDC214C74FB83D4B28BC41F9B504CCE21351493335057A9C461E79F3FD6CA55D6DFA504540AA65469E4A7D69BD8AF49B439668FC4C537F88F184735E255DB68221AAA35314D6D3622EA72DE9BD07589F22590F68CF293630321482B33E721C3EF30C54C9009300007CA16183859E0366BBE257C17E6FC829E94E0D18D397516A025FEC8B671260DC499DD8FCBD783F75446AEE3C5B5C8A4CA4E9B104A156918A49209339BF6E716A9EBBE2F88FB2481396C731CED7C18F85C321C15034D43D8E035D8269A510F037107844AECF53604C340742DAF7DF7A6BE860FA19A7C939FD1C5ECDF4451E8924430495AC91FACCEF19C7A56B373986A4BEE5882C280019315982C0C0E7661ABA67945A7D8878DB3DEB8E973D804452EF2F4E58509781D85291CAD5298F251AB2AD077DA38C0119889931FF913D82D601D42B94E28040057C86F5C0DA55747EF46CCB5C0576BA67885A918A471E65D06CE555C1FBC1843D980B695193B9236352B997AC8D56D6CED8957EEACECF4B6ED6D3DB0416576AEBF6B695A8DE07B50B59ABD18BCAE9958B64E2D1A29B7540DD537EAAE778DF7C2A1EB5E83F07BD0BF86BD19980EA6472DF4E1D003334980E9BD28C54F267F1AEA33973ACA5C27558904EA1C73FD59A73CDBB5E34ED3C42A8810CA04902A604BE7CA138E062113730B90F6E0A281A916A7004A1B519D9BFCB2253532BF7DA99AD1EAED14F3281652D3AE562745C29064550DB83AC10E93D7AE91FAEC1B0FE14EC7B0247B45171A56A1EEE06D5BCA3596C0E85EEBCBD07A3E743E21A439681ADDB0244D67C206CB92BBFC050F95E173278D9FE77176A726D03EBE44AB155CFBA4C07B5662CDD3A8FBE4FB79FF08749062386E64084417D21623F190A125566A616890F482B0884F1147F7485CC8265EA035ABB26E0D0DE563A9C4AAEF594E4B790FF17BA66D1D43E286C32A03BB807906E2BC4B1E240DBCA0774D5221908F98E1FD7312FA7140EB8FDEFADE651C5AC6284BBB23956F73325259DA1DA9F2F42183552ABAE329D1621951A9EA8E99BB1832589DDB21CC4ED977CD31D1D44DF3EAAAFADB49BB531AD9A6669B18B383569BBBED46A3CB28ACBA37696977A42CCE2AC36445DD31CAA0A90C5396F6B00A39085AB10AB9A287644A20B4229F52D71D358D77CA5869C9606C42765EB66919920BDCDF3E9A3AEFC64AB248960C9015BD1C4FA7B12855B5FB209481A8CACA14A52F719A7D162785EAC36ED33294FB5C7FEB6803A8E5A2327A5321A4BA785013D6B6753D0FB9C85079592F0ECF222A0A7B67A53D2C470DAA540C48ADFC2CB55FBB24A94D8AD18BCB9272291A671794F61465EDC69236B12D589E4770F0E1B6327F8E380E0E4583C3F91FFEC427C9F349DEE01251B200354D639FF6F1D1E858496C1E4E92B113459E6FB8E099328DABFBB587AC10FA8898FB80981EB4DD30653707FE26404FDFCA68EB64326C046688156F84674CAFDD08B11A648D29010A4EF4812C8831E35410CED399FD57D2FFD49AFD7697421C58570CACE3D43AB2FE6ED9C17E49AF7BD74B7D11364C37352ABA4838EDB75595ECD2DA4D5F2B637433344316A8071F7C7B59A01BA9B89CE9D903689D8CBFBDEB2A1127536BEE5CCF3DA824D30D8AFFE4A4B8B5554C4D8A1BCE713168326E72F6F79FEFB535E761DB2A5A4DD9DA084A4DCB5AF821E2DBCACABA27FDA186A49F1B254DBD548E541912DE6F5AD43E52031A622B034B46E99AF8F4929A2207EED74CB41AB4B634BECA0E4C5FFA242FBDA4CEA8F9191B244B0D5A775ADF2D079878A447CA6B9E7FD56799A6BCA2F4ED0A5CE1FB10763B3D42EBF3574C6947B55947266873F2437D42524B3E926988860C969684A50EF94AA6015B7244F692D4A465983466AAA89A51CD4E1858D252FFE9E9EAA306E2069A94D47FAA6605364556769270A4BF9903EB497FFC03783722CB1242FC29108ADD0ADF156D667411E6B4AB489437D1DE3239823B3C7AC3E0FA805C0ED52E5C7692FFE1F209F971F2E2758FBD19BD8AF92AE630651CDCFB95109FA0EFA6F193ACAAAACCE3ABE4AE176D630A202611CF1057F46D4C7CAF90FBC2708DA98110E742F67423F6928B279CE57381F421A41D81B2E52B8EB35B1CAC7C008BAEE81C3DE2756403057E8F97C87DCE431FF520ED1B515DF6F194A0254341946194FDE11374D80B9E5EFD0F3286445503470000, N'6.5.1')
INSERT [dbo].[__MigrationHistory] ([MigrationId], [ContextKey], [Model], [ProductVersion]) VALUES (N'202411121224289_UpdatedRequestModel', N'ST10091324_PROG7312_Part1.Migrations.Configuration', 0x1F8B0800000000000400DD5CDB6EE3B6167D2FD07F10F4D4166914273D681B382DA67632303A990471A6E85BC048B44354A25C8A4A131C9C2F3B0FFDA4FE4237752529EA66D98E5A0C1044BC2C6E928B8B97BD277FFDFFCFE98F2F816F3D631691905ED893E313DBC2D40D3D42D71776CC575F7F67FFF8C3E79F4D2FBDE0C5FA252F7726CA414D1A5DD84F9C6FCE1D27729F7080A2E380B82C8CC2153F76C3C0415EE89C9E9C7CEF4C260E06081BB02C6B7A17534E029C7CC0E72CA42EDEF018F9D7A187FD284B879C65826A7D44018E36C8C517F6F27E027893B3D36F1E6EEF6EDE7F7B36397DB8458C4F8E93BAB6F5CE2708CC5A627F655B88D290230E469F7F8AF092B390AE971B4840FEFDEB0643B915F2239C75E6BC2CDEB55F27A7A25F4E59318772E38887414FC0C95936508E5E7DABE1B68B8184A1BC8421E7AFA2D7C9705ED80BEA120F536E5B7A63E7339F8982ADA37D9C631C59B5258F0ADA00BBC4BF236B16FB3C66F882E29833E41F59B7F1A34FDC9FF1EB7DF81BA61734F67DD976B01EF2940448BA65E10633FE7A8757798F3CDB72D47A8E5EB1A826D5C9FACA19B0DEB6AED1CB074CD7FC09D6C3E977B675455EB097A76474F944092C12A8C4590C9F1FC160F4E8E322DF696CF343E866935ADB32FCDAA9E5E6866688E375C85EF7DED01C472E239B8374EA1A7B045D111FDF22FEB4F7D6403658C991F731F1DAA7FB237A26EB64860D60B67587FD24337A229B54A88E45C643BE9822309B85C15DE86755CA9C877BC4D61896EC7D68CC5E8631733583A64EB9F41B0521B56E7B3110F5FF0D42D06D920D534BE1B70642FEE764077CBC0C10F177C0FA9EDDBB4551F447C89AC4724F2DCF41C3660CC3CFA27191744F822D7AC1C21508C722581F443BC412DD7D23B5EA22E9C71089C935A446627205EA6A95D8ECFCCBE75ABBCAFC8754804AC3B4AC8AF8E9F926F56B326D89D93371F11DFE3DC6518D796A998A8986EC8A99A6328384BAECF710B92E51FE0DA2BDA0FCECD42008D2C82C79C8F07B4C311362020AC041B205064E06B86D35DF13BE8FE5FC862729A1A383357511A035FEC476B1278DE4307B98235F0FDDD315B94E17B71213559D86088A8A340E51C98C59CCFB6B8B54F5D017C4438AC012A6398EF6DE0C7CAE198E8A86E6214CF0166A132DA88741B8034225F5FA29848581E816704B8C98FB74872320E050B4C308C71627125D409A4E2D5D44E45D14852E490C93AC954E976A1F2FA967351F3553EEE5C754A01FCC06D9800040E3B0CA6C7DA1DFD039F68106D63B377D5C9BA1C8455E7578A1035E47638A635B694CF944A61AF455A51D501CCCC43902F933982DD0304279559E08006E90DF381A5AAD8E6725D1D7025FCF99E30DA6A291C69E776938A778B5F1A20D6D02DA4666EA486C6A2699BE65D64D6CEDFE59CEAC7C846E9BDB7A600365F6CEDFAD985663F801A8563317A3E79A4965EB68D128B92535F4935677DE35DE32C7CEBD06E30FC0BF86B9191907D3AD16EA70A8819964C0FC51A4E217D3E91CF2B3037A941DC4741209D425E6D547A2726FAF6C771526AA20C22813404AC096CACA8350054216E616A0CAF34D054C5F711AA034116ADFE47732A990F9254D6746EB69A7E84731901576B51E52240CC9567D01AB1DECD0F9CAA5B4DAFBC64DB8D3362CD9AE70A16114EA36DEB6A1DC62088CC7EBEA30B4EE0F9D7708A90F1546370C49D39E306058F2237FA143A533DE49BDF1B9D7DEA971DB4FAFD166039748C98D9FA558CBD4873FFB7AD9DF9F1DA4188E1B19DCDA85B5454B3C64688DB55C681A2CBD222CE273C4D1231217B29917548AA9AA5B2343795BBAB056E72C97A5BC86F83D635B4707BB61B3CAC0AEA09F81D8EF92E74D832E54AB268115C847CCF09A3A0BFD38A0F55B6F7DEDD2AB2D6394A9DD91CA973E19A94CED8EA43CA4C8604A46773CCDF72C236A59DD31F323860C5677EC10CB4E9BF7CAC1A442B7CAA94EE56F2776A732B24B669B14B303ABCDD5F6C3E8D2A7ABCF4D9ADA1D29F3DACA305952778CD2052BC394A93D5685EC525556859CD1C332CDADAAD8A7E575474DBDA732569A329A35211F5E76B932A42370FFF5D154793FAB24F38BC90059D2DBE974EAD9D2A9DD07A1746B292353A4BEC56EF68FD829F433EC2E5786769FEBBF3ADA006AB5A8F405298254E75D6AC2DA35D773078E0C95A7F5D2F0CC3FA3A97796DA63E5E82E1A6501E9997D70555F8D0AABE6FD23D754E5EAA517295A2FAE60DA556B9A5D7BDAC3A82BF7A0B4886DC1F03CC3B501EE40CBD788E3E05814385EFEEECF7C923CCAE405AE11252B207FEA9FB54F4F26A75AF0F57802A19D28F27CC3B5D1140DADCED7012257E8B3602E6255C7F2C0B0E21CF88B00BD7C29A36D136D3108CCE0CF1E84670C011E84A8BA6E634A40D8133E90153146C50A197BB9B0FF9BD43FB716BF3EA41047D60D83D5716E9D58FF6B99C17E81B907E76575100686C41A892E8262FB4D9512015B3BE95B45B50E433344AA7AF0C17717A93A88E272346A0FA06DA2120FCE552276A6D6F8BE9E73A004FC8D4AFFE4C0BDAD29A607EE8D67BB18B518375D210E1F93B6B3C3C3AE29AA86950D82D243C7567E88F8AE22C71EC93650A6A8B16D90C6C4F441415D6F15C355BAAC0F1BB67588D08506DFCFC88265BA0666BD2553E4C0822D03C146CD96C657E391F1A54F70D55B72468F1F1910CC356AEEB4BEAB8E3030AAEAC9AF799ED61F789AE29ED2573038543F8630DBE9165A1F5F630A8BAA8D8A32419B8333EA03A65AE2A54C4D3444D8B404547588A73235D812C37290A0AB4A044C63248DCE0C357A62644155FDBB57A58FEE281C69D054FFAE9A096CF2FCEC2520AAFAFA0EAA27FDA913D0DD88AC4B08F1874F287615BD2BCA2CE82ACC6557B3282F527915E5C803317CC7E0FA805C0ED92E5C9B92FFCFF30BF2E3E4EDEC117B0B7A13F34DCCA1CB3878F41517A490EFA6F693A82FD5E6E94D726B8C76D105309388078D1BFA534C7CAFB0FBCA708DA98110FB42F60824E6928BC7A0F56B81F431A41D81B2E12BB6B37B1C6C7C008B6EE8123DE36D6C03027FC06BE4BEE64E947A90F68950877D3A2768CD50106518657DF8040E7BC1CB0F7F031326F99AF1470000, N'6.5.1')
INSERT [dbo].[__MigrationHistory] ([MigrationId], [ContextKey], [Model], [ProductVersion]) VALUES (N'202411121249326_UpdatedRequestModel_v2', N'ST10091324_PROG7312_Part1.Migrations.Configuration', 0x1F8B0800000000000400DD5CDB6EDC36107D2FD07F10F4D4168EE5B55B3435D629D25D3B58348E0DAF13F4CDA025EE9AA8446D29CAB551F4CBFAD04FEA2F74A82B4552B7BD59090218162F8743CEF070C899F8BF7FFE1DFFFC14F8D623661109E9993D3A3CB22D4CDDD023747966C77CF1EAB5FDF39BAFBF1A9F7BC193F5296F7722DA414F1A9DD90F9CAF4E1D27721F7080A2C380B82C8CC2053F74C3C0415EE81C1F1DFDE48C460E06081BB02C6B7C13534E029C7CC0E724A42E5EF118F997A187FD282B879A79826A7D40018E56C8C567F6FC760478A393E3EFEFAE6FAEDEFD78323ABEBB468C8F0E93BEB6F5D62708C49A637F615B88D290230E429F7E8CF09CB3902EE72B2840FEEDF30A43BB05F2239C4DE6B46CDE755E47C7625E4ED9318772E38887414FC0D149B6508EDA7DADE5B68B8584A53C8725E7CF62D6C9729ED933EA120F536E5BEA60A7139F8986ADAB7D98631C58B52D0F0AB301EB12FF0EAC49ECF398E1338A63CE907F605DC7F73E717FC5CFB7E1EF989ED1D8F765D9417AA8AB1440D1350B5798F1E71BBCC867E4D99653EDE7A81D8B6E529F6CAE9C81D5DBD6257A7A8FE9923FC07E387E6D5B17E4097B7949662E1F29814D029D388BE1F303088CEE7D5CD43B8D63BE0FDD4CA9B523C3AF9D466E1E6882385E86EC79E7034D71E432B2DACBA42EB147D005F1F135E20F3B1F0D68839536F22E265EBBBA3FA047B24C346C00B3AD1BEC2795D10359A54475282AEEF2CD1481D82C0C6E423FEB52D6DCDD22B6C4B0656F4363F53C8C99AB083476CAADDF4808A974EB9381E8FF25104137251B544BE1B70683FCE1680BF6781E20E26FC1EA7B4EEF1A45D19F216B22CB1D8D3C050E9B300C3F8BC145D12D09D698050B17401CB360B917EE105B74FB83D4B28BC41F9B504CCE21351493335057A9C461E79F3FD6CA55D6DFA504540AA65469E4A7D69BD8AF49B439668FC4C537F88F184735E255DB68221AAA35314D6D3622EA72DE9BD07589F22590F68CF293630321482B33E721C3EF30C54C9009300007CA16183859E0B6DD7C4BF82EB6F30B7A52824737E6D4598096F823DBC699341067763F2E5F0FDE5319B98E17D722932A3B6D422855A461904A26CC6CDA9F5BA4AEFBBE20EE9304E6A0E638DAF930F0B964382A069A86A0E035D8269A510F037107844AECF54B081B03D1DE70998E05C817422B6BF82B2ABD34F9345D28E66D14852E490493A4957CCFEA1CCFA967353BA2A95E7227165403644156400F3038EC415BA5812B3AC53E1889F5D64D9FDE26287291A72F2F4CC0EB284CE1D495C2940F685581BED3C6013EC24C7819C89F80B680E108E53A7911005C21BF7135945E1D3D2931D7025FAD99E215A66290C69977193837717DF0620C45016D2B3376246B6A3632F540AD536CEDE95A6A5676B0DB745B0F6C30999DDBEF5A965623F81E4CAD461783B73513CBD6994523E596A6A1FA61DDEDAEF10E3A74DB6B107E0FF6D7A09B81D9607AD4421F0E3D30930498DE8B52FC64F2DDA13E73DFA3CC49518D48A0CE31D79F90CAB35D3BEE344BAC8208A14C00A901B674AE3C1769103231B700698F3B1A98BAE314404911D5B9C9AF685223F33B9B6A19ADDE4E318F622135EB6A7552240C495675035727D861F2DA95559F7DE321DCE9189664AFD842C32AD41DBC6D4BB9C61218DD6B7D195ACF87CE27843407CDA21B96A4E94CD860597297BFE0A13254EFA4B1FA3CA6EFD404F5C79768B5820B9614E4CF4AAC791AE19FBC9AF78F76072986E34686A077216D31120F195A62A5168606492F088BF81471748FC4856CE2055AB32AEBD6D0503E964AACBACE725ACA7B88DF336BEB187E371C5619D805CC3310E75DF2F869E005BD6B9276817CC40C6FAD93D08F035A7FF4D6F72E63DE324659DA1DA97C079491CAD2EE4895671619AC52D11D4F894CCB884A5577CCDCC590C1EADC0EB1ED14BD6B8E89666E9A5757B5DF4ED69DD2C8362DDBC4981DACDADC6D37165D467C55DDA4A5DD91B298AE0C931575C72803B4324C59DA6357C801D7CAAE902B7A48A6045D2BF22975DD51D3D8AA8C95960C664FC8CECB367786E402F7DF1F4D9D77B34BB2A8990C9015BD1C4FA7712FD5B4FB209441AFCACA14A52F719A7D162785EAC36E736728F7B9FEBBA30DA0968BCA48518590EA624F4D58DBB6F53CBC2343E565BD383C8BDE28EC9D95F6D8396A00A7B281D4CA1EE7811CC9316821ADF82C779376E9529B14A317972FE59235CE2E3CEDE9D5DA0D286D625BB03C8F706180DBCFFC39E23838140D0EE77FF8139F24CF3179834B44C9422C7812B7B58F8F46C74A52F67012A49D28F27CC385D194255DD5D71E325AE82362EE03627AC079C374E31CF89B003D7D2BA3AD9385B1119821CEBD119E31357823C46AD036A604C824B107B220C66C5941604F67F65F49FF536BF6DB5D0A71605D31D81DA7D691F5778B06FB25ECEEDD2EF545D83055D668E82259B69FAA2A99B1B54A5F2BDB7533344306AB071F7C7B19AC1B99B89CA5DA03689D6CC5BDDB2A1127536BDE5F4F1D54120107C57F7242DFDA26A626F40DE7B8183419375D1EF69FABB635E761DB265A4D37DB084A4D295BF821E2DBCA28BB27FDA10CD9645F8CBD6F94D4F552395C65C87ABF695BFB485D6888FD0C2C59A66B62D64B5A8A9C58B06622D8A0ADA5F1D57860F6D227B9EA256D46CD1FD920996BD0B6D3FAAE3AC0C4283D925FF33CAD3EF334E53DA56F61E05ADF87A0EDF408ADCFAF31A545D5664599A0CDC919F509532DF952A6211A326C5A12AA3AE45399066CC961D94BD2959601D39849A35A46357B62604955FDA7A79B8F1A281C68D254FFA99A0DD814F9D9494294FE060FAC27FD2114E0DD882C4B08F1675128762B7C57B499D14598D3AE2251DE447B1BE5C803327CCBE0FA805C0ED52E5C9E92FFEBF209F971F282768FBD19BD8AF92AE630651CDCFB9510A4A0EFA6F193ACAFAACCE3ABE4EE186D630A202611CF1A57F49798F85E21F785E11A530321CE85EC2948E8928B27A1E57381F421A41D81B2E52B8EB35B1CAC7C008BAEE81C3DE2756403037E8F97C87DCE4329F520ED8AA82EFB784AD092A120CA30CAFEF00936EC054F6FFE07DF0885500F480000, N'6.5.1')
INSERT [dbo].[__MigrationHistory] ([MigrationId], [ContextKey], [Model], [ProductVersion]) VALUES (N'202411121326452_UpdatedRequestModel_v3', N'ST10091324_PROG7312_Part1.Migrations.Configuration', 0x1F8B0800000000000400DD5BDB6EE336107D2FD07F10F4D416592B4E5A741BD8BBC8DAC9C2E8E682C8BBE85B404BB44354A2BC1495DA28FA657DE827F5173AD495A2EEBEC50D165844BC1C0EC999C32167FCEFDFFF0CDEAF5C477BC6CC271E1DEAFDDEA9AE616A7936A18BA11EF0F99BB7FAFB77DF7E33B8B2DD95F62569772EDA414FEA0FF527CE971786E15B4FD8457ECF2516F37C6FCE7B96E71AC8F68CB3D3D35F8C7EDFC000A10396A60D1E02CA898BC30FF81C79D4C24B1E20E7C6B3B1E3C7E5506386A8DA2D72B1BF44161EEAE6B40F78FDF3B31F1FEF1FEE3EFE7CDE3F7BBC478CF77B615F5DBB740802B14CECCC750D51EA71C441E88BCF3E3639F3E8C25C420172A6EB25867673E4F8389ECC45D6BCEDBC4ECFC4BC8CAC630265053EF7DC8E80FDF378A10CB5FB46CBADA70B094B79054BCED762D6E1720EF509B5888D29D73575B08B91C344C3C6D5EE2518275A65CB93546D40BBC4BF136D14383C60784871C019724EB4FB60E610EB57BC9E7ABF633AA481E3C8B283F450972B80A27BE62D31E3EB073C4F6664EB9A91EF67A81DD36E529F78AE9C81D6EBDA0D5A7DC274C19FC01ECEDEEADA3559613B2989D5E533256024D089B3003E6F41603473705A6FD48EF9C9B3E24DAD1C19FE6C3572FD4023C4F1C263EBBD0F34C6BEC5C8F22093BAC13641D7C4C1F7883FED7D34A00D96E9C8C780D8CDDB7D8B9EC922DCE112305D7BC04E58E93F916544543D51F19818930F6233CF7DF09CB84B56F338456C81C164A75E69B5E905CC52041A1899E9D7124224DDE66420FABF062268B7C9255B4BE1AF1A85FCE97407FA78E522E2EC40EB3B4EEF1EF9FE1F1EAB23CB3D8D3C060E1B310CFFA7838BA22971379805F3E6401C13777110EE1026BAFB412AD945E28F6D2826E1900A8A4918A8AD542666CFC4C20FF86B80FD0AD9F26D1E2322CA042CA92E1061599BADD8300FB80D2FE6918E8321636126E3EE4429753DB4E3744837C3846D0EFCBD0F039F0B86FD74A0B1071BDC7D6526FE84DA9863E6120AEC98A07DF0C03810ED0C6762C4ACA778A705D42BF1B336201A950EEBC86823A211B701E7EA79CB9B5886721C04B39D0B36A1FCFCAC4401A49531B9C7F0474C3113AE019CE7A0FC5460E070819BF46D4AF83E0EE71724ACB164F71B7B4813172DF067B60B0FF345AEA6555677E9FB9E4542CD913842F287F2C25E515BAB778E22A113C70AE4063B224BB01C181CCE3F5DB5903B3AC60E10B4766945CF4123E45BC82EAE134CC06E294CEA0365C2648F3A79817E288C03A68A99B015E48C8023C1F809E545BB2600B8444EED6A28BD5AF281986B8AAFD68CF112533148EDCCDB0C9C1C2CC5C1D331940D685A9981216953BD92959D37559B5B7BF8643BACFAA54DFB5C3F40890AED5D9F37D2BC1AE10FA07E357B73643A18D11FF4E1D003334980F14C94E255999B01F5B1A7E1C7ACAA2A914035312F5E3533BE2D50504113F32042A8328048011B3A17AE950518D5501A003307AA0C4C76D214206923F273936FDB52A3F2FBB8AA198D27502A7FBA9005ED6A3C38240C4956D580F3136C31F9522FBBB8028DE4D89A1EA57914F4A26655EA08B169796B9625F1415223CCE2594614D04A025F4645E46B7083964B7087A448585CA29951186CF4C6EC1E1272230CC3F24B2243A9B4E948E0608333A8D4C2D020E935613E072713CD9070C846B65B6896A79C0ADB4BC65259A5B867893D263DC4DFB1B6B58C519530750C760DF37405D987778A12A328760D6393C841ACE40A33F29CC0A5D5E74E75EF2C30246364A5ED9132F75A46CA4ADB23E5AE303258AEA23D9E12BE911195AAF698C9F92A83559DB9C2EC947D2F9CCA05752BB83479FD6DA5DD118DEC52B3CB18B385569777DB8F46676111756FA2D2F64871E04386898BDA6364510C19262BED60157254226715724507C994C8444E3EA5AE3D6A148090B1A292A3B109F5B4DEA57528FE5F773B6902A85CF5EC813CB7F4554FEEB57AB663F64D5EB565A8A4AC93B6C68FD68A9EC6A5ED910AEFD639AA512B3BCCB3F8809D9B72B1FA959D36F275669756255D84BA5B545DE7FD9C3FF133AF0C1017BD9C0D8E0B8A3EEEA8DDD92B6D6E65D2D27DFA897BD5E7C2D5496D928E9E5EA194ABD220BEB634671216EE3151135D83257A06B71FEE30E6DAE7D8ED89063DF3AB337248F8009034B84194CC0581849112FDECB47FA6E41F1E4F2EA0E1FBB6D33221F0E0E11EFA2CE818B1620C79CBCCBA04F83B17ADBE97D13609516C05561209DA0AAF340B6E2BC47C0436A0040EC7501FC89C94268689C37935D4FF0CFB5F6893DF1E238813ED8E81755C68A7DA5F0D3BD82D37EDE07A595C842DB3C24A155DE48575DBAA5C1258E5A66F94D8B51D5A49B2960D1F7C77C95A5BA9B89C90D50168D39CA10DF4759779393B63D55D73573EB5662B28357D66EE7888EF2A7B6646BA435566CEBC5E6EAEF2EA0FC1D044F8638DA9201DF730971B7254A7BE9CE3B131B1AA391E2FE524ED2557E3A55233B2A8E761B3310E11FDAE89A0FC8FF32D5E525BD4D8F416F91D47AD398D6FB047982B518C6F563CB8157E4757930A11BD2D0069CF3CD8ED8811AB43EE65991295891265D0E521EBFA1C8A16291465433504DC6BF22C1AD22CCA06CB2569BC401E46212FA036BF40D58C7C4CF948F32CBA4FB35C8DCA42287BC9A1283EF80125483F300652F2C92283103F37A6D8CA9141DA6642E75EC2498A444993C2430C47E08AA14B06CE38B238545B7021099359BF202708AFEB336C4FE85DC0970187296377E6E4DE5605B7D58D1F268AE4651EDC855EA5BF8B29809844789377F443401C3B95FBBAE4525001214833F6C0C55E72E1892FD629D2AD475B02C5CB9772FD14BB4B07C0FC3B6AA267BC896CA0C09FF00259EBE4DDB61AA47923F2CB3E1813B460C8F5638CAC3F7C820EDBEEEADD7F8D21A98C673F0000, N'6.5.1')
GO
INSERT [dbo].[Incidents] ([Id], [Location], [Category], [Description], [MediaFilePath], [UserId]) VALUES (N'ALEX1', N'Alexandra', N'Public Space', N'Mass shooting in alex
', N'C:\Users\Zozz\Downloads\person - Copy.png', N'e0ee88eb-0ac1-44fe-9c19-0ef9c754d772')
INSERT [dbo].[Incidents] ([Id], [Location], [Category], [Description], [MediaFilePath], [UserId]) VALUES (N'BRAM1', N'Bramley', N'Roads', N'Massive potholes on ramley st
', N'C:\Users\Zozz\Pictures\Saved Pictures\F1\Alfa Romeo\2023-Formula1-Alfa-Romeo-C43-001-2160.jpg', N'e0ee88eb-0ac1-44fe-9c19-0ef9c754d772')
INSERT [dbo].[Incidents] ([Id], [Location], [Category], [Description], [MediaFilePath], [UserId]) VALUES (N'EDEN3', N'Edenvale', N'Transportation', N'Slow moving traffic on R59
', N'C:\Users\Zozz\Pictures\Saved Pictures\F1\Mclaren\2024-dutch-wallpaper-desktop.jpg', N'e0ee88eb-0ac1-44fe-9c19-0ef9c754d772')
INSERT [dbo].[Incidents] ([Id], [Location], [Category], [Description], [MediaFilePath], [UserId]) VALUES (N'LINK4', N'Linksfield', N'Public Space', N'Burst pipe near hade park
', N'C:\Users\Zozz\Pictures\Saved Pictures\F1\Racing Bulls\2024-Formula1-Racing-Bulls-RB01-001-2160.jpg', N'e0ee88eb-0ac1-44fe-9c19-0ef9c754d772')
GO
INSERT [dbo].[Users] ([Id], [Username], [Email], [Password], [DateCreated], [ProfileImgPath], [Role]) VALUES (N'5f402a2f-ee48-4109-8b2c-0d520ed003fa', N'Kevin11', N'kevin.melvin@gmail.com', N'fuJ59rhL7loHcTwKOFFXuE04FYFzixekAWOVXS6vOO43Zsoj', CAST(N'2024-11-11T13:42:01.373' AS DateTime), NULL, N'Technician')
INSERT [dbo].[Users] ([Id], [Username], [Email], [Password], [DateCreated], [ProfileImgPath], [Role]) VALUES (N'e0ee88eb-0ac1-44fe-9c19-0ef9c754d772', N'tester2', N'tester2@gmail.com', N'9cXSXORjz3i/Eme9GOJC8TetHJ01ma1vGPyZRI+6NKqqnZfK', CAST(N'2024-11-11T13:39:54.277' AS DateTime), N'C:\Users\Zozz\Pictures\Saved Pictures\F1\Renault\2020-Formula1-Renault-RS20-002-2160.jpg', N'Admin')
INSERT [dbo].[Users] ([Id], [Username], [Email], [Password], [DateCreated], [ProfileImgPath], [Role]) VALUES (N'7ad1e24c-5a38-48e2-9121-6d295cd4a33b', N'mrs_shozi', N'shozi.teacher@gmail.com', N'6fIrbSruyCN1TpuJVqHusgGqKTP6eRPt5hWXzqI2Hx3Z601d', CAST(N'2024-11-11T13:38:32.573' AS DateTime), NULL, N'Admin')
INSERT [dbo].[Users] ([Id], [Username], [Email], [Password], [DateCreated], [ProfileImgPath], [Role]) VALUES (N'c89b2600-0b2b-4252-9702-d7b34af1bebc', N'Leee', N'lesegoM@icloud.com', N'1UEgyFre7NfzimCElbnkEwMOv+J+cjKWC9riFYSj8ouxhuPh', CAST(N'2024-11-11T13:46:27.397' AS DateTime), NULL, N'Customer')
GO
/****** Object:  Index [IX_UserId]    Script Date: 11/18/2024 7:29:23 PM ******/
CREATE NONCLUSTERED INDEX [IX_UserId] ON [dbo].[Incidents]
(
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_UserId]    Script Date: 11/18/2024 7:29:23 PM ******/
CREATE NONCLUSTERED INDEX [IX_UserId] ON [dbo].[ServiceRequests]
(
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Incidents]  WITH CHECK ADD  CONSTRAINT [FK_dbo.Incidents_dbo.Users_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[Users] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Incidents] CHECK CONSTRAINT [FK_dbo.Incidents_dbo.Users_UserId]
GO
ALTER TABLE [dbo].[ServiceRequests]  WITH CHECK ADD  CONSTRAINT [FK_dbo.ServiceRequests_dbo.Users_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[Users] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[ServiceRequests] CHECK CONSTRAINT [FK_dbo.ServiceRequests_dbo.Users_UserId]
GO
USE [master]
GO
ALTER DATABASE [JoziIncident] SET  READ_WRITE 
GO
