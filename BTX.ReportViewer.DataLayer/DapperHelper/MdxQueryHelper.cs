using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BTX.ReportViewer.DataLayer.DapperHelper
{
    class MdxQueryHelper
    {
        public const string PeriodKeys = @"
                WITH
                MEMBER [Measures].[Period Label] AS [Date].[Promotional Period].CURRENTMEMBER.Properties('ID',TYPED)
                MEMBER [Measures].[ParameterCaption] AS [Date].[Promotional Period].CURRENTMEMBER.MEMBER_CAPTION 
                SELECT
                [Measures].[ParameterCaption] ON 0
                ,NONEMPTY(Order([Date].[Promotional Period].[Promotional Period].members, [Measures].[Period Label], BDESC), [Measures].[Units]) ON 1
                FROM
                [LCBO Week]
                ";

        public const string SetNames = @"
                WITH
                MEMBER [Measures].[Category Code] AS [Product].[Category].member_value
                SELECT
                [Measures].[Category Code] ON 0
                , Filter([Product].[Category].[Category].members, NOT ISEMPTY([Product].[Category])) ON 1
                FROM
                [LCBO Week]
                ";

        public const string UnitSizes = @"
                WITH
                MEMBER [Measures].[Unit Code] AS [Product].[Volume Per Unit ML].member_value
                SELECT
                [Measures].[Unit Code] ON 0
                , Order([Product].[Volume Per Unit ML].[Volume Per Unit ML].members, [Measures].[Unit Code], ASC) ON 1
                FROM
                [LCBO Week]
                WHERE
                (
                {@SetNames}
                , @Period
                )
                ";

        public const string MyCategory = @"
                WITH
                MEMBER [Measures].[ParameterCaption] AS [Product].[My Category].CURRENTMEMBER.MEMBER_CAPTION 
                SELECT
                [Measures].[ParameterCaption] ON 0
                ,[Product].[My Category].[My Category].members ON 1
                FROM
                [LCBO Week]
        ";

        public static string Top50Sales(decimal pricefrom, decimal priceto)
        {
            return @"
                        WITH 
                        SET AgentName AS {TOPCOUNT(Filter([Product].[Agent].[Agent].members, [Measures].[Average Sale Price]>" + pricefrom + @" and [Measures].[Average Sale Price]<" + priceto + @"), 50, [Measures].[Sales Amount MAT])}
                        MEMBER [Measures].[xRank] AS Rank([Product].[Agent].CurrentMember, AgentName)  
                        
                        MEMBER[Measures].[9L Eq Cases CP LY Var %] AS IIF(Round([9L Eq Cases CP LY],0) = 0, null, (Round([9L Eq Cases CP TY],0) - Round([Measures].[9L Eq Cases CP LY],0)) / Round([Measures].[9L Eq Cases CP LY],0)),format_string='Percent',visible = 1                     

                        //MEMBER[Measures].[9L Eq Cases MAT] AS SUM({[Date].[Promotional Period].currentmember.lag(12):[Date].[Promotional Period].currentmember},[Measures].[9L Eq Cases]), FORMAT_STRING = '#,##0;(#,##0),0'
                        //MEMBER[Measures].[9L Eq Cases Prior MAT] AS SUM({[Date].[Promotional Period].currentmember.lag(25):[Date].[Promotional Period].currentmember.lag(13)}, [Measures].[9L Eq Cases]), FORMAT_STRING = '#,##0;(#,##0),0'
                        MEMBER[Measures].[9L Eq Cases Prior MAT Var %] AS IIF(round([Measures].[9L Eq Cases Prior MAT],0) = 0, null, (round([Measures].[9L Eq Cases MAT],0) - round([Measures].[9L Eq Cases Prior MAT],0)) / round([Measures].[9L Eq Cases Prior MAT],0)),format_string='Percent',visible = 1

                        MEMBER[Measures].[On Prem 13 Per TY] AS SUM({[Date].[Promotional Period].currentmember.lag(12):[Date].[Promotional Period].currentmember},[Measures].[Units])
                        MEMBER[Measures].[On Prem 13 Per LY] AS SUM({[Date].[Promotional Period].currentmember.lag(25):[Date].[Promotional Period].currentmember.lag(13)}, [Measures].[Units])
                        MEMBER[Measures].[On Prem 13 Var %] AS IIF([Measures].[On Prem 13 Per TY] = 0, null, ([Measures].[On Prem 13 Per TY] - [Measures].[On Prem 13 Per LY]) / [Measures].[On Prem 13 Per TY]),format_string='Percent',visible = 1
        
                        MEMBER[Measures].[Rol Year Per TY$] AS SUM({[Date].[Promotional Period].currentmember.lag(12):[Date].[Promotional Period].currentmember}, [Measures].[Sales Amount])
                        MEMBER[Measures].[Rol Year $ Var %] AS IIF([Measures].[Rol Year Per TY$] = 0, null, ([Measures].[Rol Year Per TY$]-SUM({[Date].[Promotional Period].currentmember.lag(25):[Date].[Promotional Period].currentmember.lag(13)},[Measures].[Sales Amount])) / [Measures].[Rol Year Per TY$]), format_string='Percent',visible = 1

                        SELECT 
                        {
                        [Measures].[xRank],
                        [Measures].[9L Eq Cases CP TY],[Measures].[9L Eq Cases CP LY],[Measures].[9L Eq Cases CP LY Var %],
                        [Measures].[Sales Amount CP TY],[Measures].[Sales Amount CP LY],[Measures].[Sales Amount CP LY Var %],
                        [Measures].[Units CP TY],[Measures].[Units CP LY],[Measures].[Units CP LY Var %], 

                        [Measures].[9L Eq Cases P6P],[Measures].[9L Eq Cases Prior P6P],[Measures].[9L Eq Cases Prior P6P Var %],
                        [Measures].[Sales Amount P6P],[Measures].[Sales Amount Prior P6P],[Measures].[Sales Amount Prior P6P Var %],
                        [Measures].[Units P6P],[Measures].[Units Prior P6P],[Measures].[Units Prior P6P Var %],

                        [Measures].[9L Eq Cases MAT],[Measures].[9L Eq Cases Prior MAT],[Measures].[9L Eq Cases Prior MAT Var %],
                        [Measures].[Sales Amount MAT],[Measures].[Sales Amount Prior MAT],[Measures].[Sales Amount Prior MAT Var %],
                        [Measures].[Units MAT],[Measures].[Units Prior MAT],[Measures].[Units Prior MAT Var %],

                        [Measures].[Rol Year Per TY$],[Measures].[Rol Year $ Var %],
                        [Measures].[On Prem 13 Per TY], [Measures].[On Prem 13 Per LY] , [Measures].[On Prem 13 Var %]
                        } on 0,

                        AgentName on 1

                        FROM [LCBO Week]
                        WHERE 
                        (
                        @Period,
                        {@Category},
                        {@UnitSize}
                        )
                        ";
        }

        public static string SalesByAgents(decimal pricefrom, decimal priceto)
        {
            return @"
                        WITH
                        MEMBER [Measures].[Nb_General] as 0
                        MEMBER [Measures].[Nb_Vintages] as 0
                        MEMBER [Measures].[Nb_VintageEssential] as 0
                                      
                        MEMBER[Measures].[9L Eq Cases CP LY Var %] AS IIF(Round([9L Eq Cases CP LY],0) = 0, null, (Round([9L Eq Cases CP TY],0) - Round([Measures].[9L Eq Cases CP LY],0)) / Round([Measures].[9L Eq Cases CP LY],0)),format_string='Percent',visible = 1              
                                
                        //MEMBER[Measures].[9L Eq Cases MAT] AS SUM({[Date].[Promotional Period].currentmember.lag(12):[Date].[Promotional Period].currentmember},[Measures].[9L Eq Cases]), FORMAT_STRING = '#,##0;(#,##0),0'
                        //MEMBER[Measures].[9L Eq Cases Prior MAT] AS SUM({[Date].[Promotional Period].currentmember.lag(25):[Date].[Promotional Period].currentmember.lag(13)}, [Measures].[9L Eq Cases]), FORMAT_STRING = '#,##0;(#,##0),0'
                        //MEMBER[Measures].[9L Eq Cases Prior MAT Var %] AS IIF(round([Measures].[9L Eq Cases Prior MAT],0) = 0, null, (round([Measures].[9L Eq Cases MAT],0) - round([Measures].[9L Eq Cases Prior MAT],0)) / round([Measures].[9L Eq Cases Prior MAT],0)),format_string='Percent',visible = 1

                        MEMBER[Measures].[On Prem 13 Per TY] AS SUM({[Date].[Promotional Period].currentmember.lag(12):[Date].[Promotional Period].currentmember},[Measures].[Units])
                        MEMBER[Measures].[On Prem 13 Per LY] AS SUM({[Date].[Promotional Period].currentmember.lag(25):[Date].[Promotional Period].currentmember.lag(13)}, [Measures].[Units])
                        MEMBER[Measures].[On Prem 13 Var %] AS IIF([Measures].[On Prem 13 Per TY] = 0, null, ([Measures].[On Prem 13 Per TY] - [Measures].[On Prem 13 Per LY]) / [Measures].[On Prem 13 Per TY]),format_string='Percent',visible = 1
                        MEMBER[Measures].[Rol Year Per TY$] AS SUM({[Date].[Promotional Period].currentmember.lag(12):[Date].[Promotional Period].currentmember}, [Measures].[Sales Amount])
                        MEMBER[Measures].[Rol Year $ Var %] AS IIF([Measures].[Rol Year Per TY$] = 0, null, ([Measures].[Rol Year Per TY$]-SUM({[Date].[Promotional Period].currentmember.lag(25):[Date].[Promotional Period].currentmember.lag(13)},[Measures].[Sales Amount])) / [Measures].[Rol Year Per TY$]), format_string='Percent',visible = 1
                        SELECT 
                        {        
                        [Measures].[Nb_General],
                        [Measures].[Nb_Vintages],
                        [Measures].[Nb_VintageEssential], 
                            -----TY&LY----------------------
                        [Measures].[9L Eq Cases CP TY],[Measures].[9L Eq Cases CP LY],[Measures].[9L Eq Cases CP LY Var %],
                        [Measures].[Sales Amount CP TY],[Measures].[Sales Amount CP LY],[Measures].[Sales Amount CP LY Var %],
                        [Measures].[Units CP TY] ,[Measures].[Units CP LY],[Measures].[Units CP LY Var %],
                            -----6TY&LY----------------------
                        [Measures].[9L Eq Cases P6P],[Measures].[9L Eq Cases Prior P6P],[Measures].[9L Eq Cases Prior P6P Var %],
                        [Measures].[Sales Amount P6P],[Measures].[Sales Amount Prior P6P],[Measures].[Sales Amount Prior P6P Var %],
                        [Measures].[Units P6P],[Measures].[Units Prior P6P],[Measures].[Units Prior P6P Var %],
                        ----------1year-----------------
                        //[Measures].[9L Eq Cases MAT] ,[Measures].[9L Eq Cases Prior MAT],[Measures].[9L Eq Cases Prior MAT Var %],
                        //[Measures].[Sales Amount MAT],[Measures].[Sales Amount Prior MAT],[Measures].[Sales Amount Prior MAT Var %],
                        //[Measures].[Units MAT],[Measures].[Units Prior MAT],[Measures].[Units Prior MAT Var %],
                            -----------------------------------
                        [Measures].[On Prem 13 Per TY] ,
                        [Measures].[On Prem 13 Per LY],
                        [Measures].[On Prem 13 Var %],
                        [Measures].[Rol Year Per TY$],
                        [Measures].[Rol Year $ Var %]                   
                        } on columns,
                        [Product].[Brand].[Brand].members
                        having[Measures].[Average Sale Price] >0 and[Measures].[Average Sale Price] <10000
                        on rows
                        FROM[LCBO Week]
                        WHERE
                        (
                        @Period,
                        {@Category},
                        {@UnitSize},
                        @Agent
                        )
                        ";
        }

        public static string SalesByProducts(decimal pricefrom, decimal priceto)
        {
            return @"
                        WITH
                        MEMBER [Measures].[Nb_General] as 0
                        MEMBER [Measures].[Nb_Vintages] as 0
                        MEMBER [Measures].[Nb_VintageEssential] as 0
                        
                        MEMBER[Measures].[9L Eq Cases CP LY Var %] AS IIF(Round([9L Eq Cases CP LY],0) = 0, null, (Round([9L Eq Cases CP TY],0) - Round([Measures].[9L Eq Cases CP LY],0)) / Round([Measures].[9L Eq Cases CP LY],0)),format_string='Percent',visible = 1   

                        //MEMBER[Measures].[9L Eq Cases MAT] AS SUM({[Date].[Promotional Period].currentmember.lag(12):[Date].[Promotional Period].currentmember},[Measures].[9L Eq Cases]), FORMAT_STRING = '#,##0;(#,##0),0'
                        //MEMBER[Measures].[9L Eq Cases Prior MAT] AS SUM({[Date].[Promotional Period].currentmember.lag(25):[Date].[Promotional Period].currentmember.lag(13)}, [Measures].[9L Eq Cases]), FORMAT_STRING = '#,##0;(#,##0),0'
                        //MEMBER[Measures].[9L Eq Cases Prior MAT Var %] AS IIF(round([Measures].[9L Eq Cases Prior MAT],0) = 0, null, (round([Measures].[9L Eq Cases MAT],0) - round([Measures].[9L Eq Cases Prior MAT],0)) / round([Measures].[9L Eq Cases Prior MAT],0)),format_string='Percent',visible = 1

                        MEMBER[Measures].[On Prem 13 Per TY] AS SUM({[Date].[Promotional Period].currentmember.lag(12):[Date].[Promotional Period].currentmember},[Measures].[Units])
                        MEMBER[Measures].[On Prem 13 Per LY] AS SUM({[Date].[Promotional Period].currentmember.lag(25):[Date].[Promotional Period].currentmember.lag(13)}, [Measures].[Units])
                        MEMBER[Measures].[On Prem 13 Var %] AS IIF([Measures].[On Prem 13 Per TY] = 0, null, ([Measures].[On Prem 13 Per TY] - [Measures].[On Prem 13 Per LY]) / [Measures].[On Prem 13 Per TY]),format_string='Percent',visible = 1
                        MEMBER[Measures].[Rol Year Per TY$] AS SUM({[Date].[Promotional Period].currentmember.lag(12):[Date].[Promotional Period].currentmember}, [Measures].[Sales Amount])
                        MEMBER[Measures].[Rol Year $ Var %] AS IIF([Measures].[Rol Year Per TY$] = 0, null, ([Measures].[Rol Year Per TY$]-SUM({[Date].[Promotional Period].currentmember.lag(25):[Date].[Promotional Period].currentmember.lag(13)},[Measures].[Sales Amount])) / [Measures].[Rol Year Per TY$]), format_string='Percent',visible = 1
                        MEMBER[Measures].[Promo] as  Generate(existing (filter( [Promotion].[Promotion Code].[Promotion Code].members , [Measures].[Promotion Count]>=1) ) ,[Promotion].[Promotion Code]. CurrentMember.Name,', ')

                        SELECT 
                        {
                        [Measures].[Average Sale Price],
                        [Measures].[Nb_General],
                        [Measures].[Nb_Vintages],
                        [Measures].[Nb_VintageEssential], 

                            -----TY&LY----------------------
                        [Measures].[9L Eq Cases CP TY],[Measures].[9L Eq Cases CP LY],[Measures].[9L Eq Cases CP LY Var %],
                        [Measures].[Sales Amount CP TY],[Measures].[Sales Amount CP LY],[Measures].[Sales Amount CP LY Var %],
                        [Measures].[Units CP TY] ,[Measures].[Units CP LY],[Measures].[Units CP LY Var %],
                            -----6TY&LY----------------------
                        [Measures].[9L Eq Cases P6P],[Measures].[9L Eq Cases Prior P6P],[Measures].[9L Eq Cases Prior P6P Var %],
                        [Measures].[Sales Amount P6P],[Measures].[Sales Amount Prior P6P],[Measures].[Sales Amount Prior P6P Var %],
                        [Measures].[Units P6P],[Measures].[Units Prior P6P],[Measures].[Units Prior P6P Var %],
                        ----------1year-----------------
                        //[Measures].[9L Eq Cases MAT] ,[Measures].[9L Eq Cases Prior MAT],[Measures].[9L Eq Cases Prior MAT Var %],
                        //[Measures].[Sales Amount MAT],[Measures].[Sales Amount Prior MAT],[Measures].[Sales Amount Prior MAT Var %],
                        //[Measures].[Units MAT],[Measures].[Units Prior MAT],[Measures].[Units Prior MAT Var %],
                            -----------------------------------
                        [Measures].[On Prem 13 Per TY] ,
                        [Measures].[On Prem 13 Per LY],
                        [Measures].[On Prem 13 Var %],
                        [Measures].[Rol Year Per TY$],
                        [Measures].[Rol Year $ Var %],
                        [Measures].[Promo]                 
                        } on columns,
                        [Product].[CSPC].[CSPC].members
                        *[Product].[Product Name].[Product Name].members      
                        *[Product].[Volume Per Unit ML].[Volume Per Unit ML].members
                        *[Product].[Units Per Case].[Units Per Case].members
                        having  [Measures].[Average Sale Price] >" + pricefrom + @" and [Measures].[Average Sale Price] <" + priceto +
                        @"
                        on rows
                        FROM [LCBO Week]
                        WHERE
                        (
                        @Period,
                        {@Category},
                        --{@UnitSize},
                        {@Agent},
                        {@Brand}
                        )
                        ";
        }


        public static string SalesSummaryByColor(decimal pricefrom, decimal priceto)
        {
            return @"
                        WITH
                        MEMBER [Measures].[Nb_General] as 0
                        MEMBER [Measures].[Nb_Vintages] as 0
                        MEMBER [Measures].[Nb_VintageEssential] as 0
                        //MEMBER [Measures].[9L Eq Cases CP TY] AS [Measures].[9L Eq Cases], FORMAT_STRING = '#,##0;(#,##0),0'
                        //MEMBER[Measures].[9L Eq Cases CP LY] AS(ParallelPeriod([Date].[Promotional Period].CurrentMember.Level, 13,[Date].[Promotional Period].CurrentMember),[Measures].[9L Eq Cases]), FORMAT_STRING = '#,##0;(#,##0),0'
                        MEMBER[Measures].[9L Eq Cases CP LY Var %] AS IIF(Round([9L Eq Cases CP LY],0) = 0, null, (Round([9L Eq Cases CP TY],0) - Round([Measures].[9L Eq Cases CP LY],0)) / Round([Measures].[9L Eq Cases CP LY],0)),format_string='Percent',visible = 1

                        //MEMBER[Measures].[9L Eq Cases MAT] AS SUM({[Date].[Promotional Period].currentmember.lag(12):[Date].[Promotional Period].currentmember},[Measures].[9L Eq Cases]), FORMAT_STRING = '#,##0;(#,##0),0'
                        //MEMBER[Measures].[9L Eq Cases Prior MAT] AS SUM({[Date].[Promotional Period].currentmember.lag(25):[Date].[Promotional Period].currentmember.lag(13)}, [Measures].[9L Eq Cases]), FORMAT_STRING = '#,##0;(#,##0),0'
                        //MEMBER[Measures].[9L Eq Cases Prior MAT Var %] AS IIF(round([Measures].[9L Eq Cases Prior MAT],0) = 0, null, (round([Measures].[9L Eq Cases MAT],0) - round([Measures].[9L Eq Cases Prior MAT],0)) / round([Measures].[9L Eq Cases Prior MAT],0)),format_string='Percent',visible = 1

                        //MEMBER[Measures].[9L Eq Cases YTD] AS SUM(PeriodsToDate([Date].[Promotional].[Year], [Date].[Promotional].currentmember),[Measures].[9L Eq Cases]), FORMAT_STRING = '#,##0;(#,##0),0'
                        //MEMBER[Measures].[9L Eq Cases Prior YTD] AS SUM(PeriodstoDate([Date].[Promotional].[Year], ParallelPeriod([Date].[Promotional].[Year], 1, [Date].[Promotional].currentmember)),[Measures].[9L Eq Cases]), FORMAT_STRING = '#,##0;(#,##0),0'
                        MEMBER[Measures].[9L Eq Cases Prior YTD Var %] AS IIF(Round([Measures].[9L Eq Cases Prior YTD],0) = 0, null, (Round([Measures].[9L Eq Cases YTD],0) - Round([Measures].[9L Eq Cases Prior YTD],0)) / Round([Measures].[9L Eq Cases Prior YTD],0)),format_string='Percent',visible = 1

                        MEMBER[Measures].[On Prem 13 Per TY] AS SUM({[Date].[Promotional Period].currentmember.lag(12):[Date].[Promotional Period].currentmember},[Measures].[Units])
                        MEMBER[Measures].[On Prem 13 Per LY] AS SUM({[Date].[Promotional Period].currentmember.lag(25):[Date].[Promotional Period].currentmember.lag(13)}, [Measures].[Units])
                        MEMBER[Measures].[On Prem 13 Var %] AS IIF([Measures].[On Prem 13 Per TY] = 0, null, ([Measures].[On Prem 13 Per TY] - [Measures].[On Prem 13 Per LY]) / [Measures].[On Prem 13 Per TY]),format_string='Percent',visible = 1

                        MEMBER[Measures].[Rol Year Per TY$] AS SUM({[Date].[Promotional Period].currentmember.lag(12):[Date].[Promotional Period].currentmember}, [Measures].[Sales Amount])
                        MEMBER[Measures].[Rol Year $ Var %] AS IIF([Measures].[Rol Year Per TY$] = 0, null, ([Measures].[Rol Year Per TY$]-SUM({[Date].[Promotional Period].currentmember.lag(25):[Date].[Promotional Period].currentmember.lag(13)},[Measures].[Sales Amount])) / [Measures].[Rol Year Per TY$]), format_string='Percent',visible = 1

                        MEMBER[Measures].[Agent 9L Eq Cases MAT] as ([Measures].[9L Eq Cases MAT], [Account].[Channel].&[OFF TRADE]  )
                        MEMBER[Measures].[Lic 9L Eq Cases MAT] as ([Measures].[9L Eq Cases MAT], [Account].[Channel].&[ON TRADE]  )  
                        MEMBER[Measures].[Agent 9L Eq Cases MAT Pct] as ([Measures].[9L Eq Cases Prior MAT Var %] , [Account].[Channel].&[OFF TRADE]  )
                        MEMBER[Measures].[Lic 9L Eq Cases MAT Pct] as ([Measures].[9L Eq Cases Prior MAT Var %], [Account].[Channel].&[ON TRADE]  ) 
                        MEMBER[Measures].[Agent Sales Amount MAT] as ([Measures].[Sales Amount MAT] , [Account].[Channel].&[OFF TRADE]  )
                        MEMBER[Measures].[Lic Sales Amount MAT] as ( [Measures].[Sales Amount MAT], [Account].[Channel].&[ON TRADE]  )
                        MEMBER[Measures].[Agent Sales Amount MAT Pct] as ([Measures].[Sales Amount Prior MAT Var %] , [Account].[Channel].&[OFF TRADE]  )
                        MEMBER[Measures].[Lic Sales Amount MAT Pct] as ([Measures].[Sales Amount Prior MAT Var %], [Account].[Channel].&[ON TRADE]  )
                        MEMBER[Measures].[Agent Units MAT] as ( [Measures].[Units MAT] , [Account].[Channel].&[OFF TRADE]  )
                        MEMBER[Measures].[Lic Units MAT] as ( [Measures].[Units MAT] , [Account].[Channel].&[ON TRADE]  )    
                        MEMBER[Measures].[Agent Units MAT Pct] as ( [Measures].[Units Prior MAT Var %]  , [Account].[Channel].&[OFF TRADE]  )
                        MEMBER[Measures].[Lic Units MAT Pct] as (  [Measures].[Units Prior MAT Var %] , [Account].[Channel].&[ON TRADE]  )         

                        SELECT 
                        {
                            -----TY&LY----------------------
                        [Measures].[9L Eq Cases CP TY],[Measures].[9L Eq Cases CP LY],[Measures].[9L Eq Cases CP LY Var %],
                        [Measures].[Sales Amount CP TY],[Measures].[Sales Amount CP LY],[Measures].[Sales Amount CP LY Var %],
                        [Measures].[Units CP TY] ,[Measures].[Units CP LY],[Measures].[Units CP LY Var %],
                            -----6TY&LY----------------------
                        [Measures].[9L Eq Cases P6P],[Measures].[9L Eq Cases Prior P6P],[Measures].[9L Eq Cases Prior P6P Var %],
                        [Measures].[Sales Amount P6P],[Measures].[Sales Amount Prior P6P],[Measures].[Sales Amount Prior P6P Var %],
                        [Measures].[Units P6P],[Measures].[Units Prior P6P],[Measures].[Units Prior P6P Var %],
                            ----------1year-----------------
                        //[Measures].[9L Eq Cases MAT] ,[Measures].[9L Eq Cases Prior MAT],[Measures].[9L Eq Cases Prior MAT Var %],
                        //[Measures].[Sales Amount MAT],[Measures].[Sales Amount Prior MAT],[Measures].[Sales Amount Prior MAT Var %],
                        //[Measures].[Units MAT],[Measures].[Units Prior MAT],[Measures].[Units Prior MAT Var %],
                            ----------ytd----------------------
                        [Measures].[9L Eq Cases YTD],[Measures].[9L Eq Cases Prior YTD],[Measures].[9L Eq Cases Prior YTD Var %],
                           [Measures].[Sales Amount YTD],[Measures].[Sales Amount Prior YTD],[Measures].[Sales Amount Prior YTD Var %],
                        [Measures].[Units YTD],[Measures].[Units Prior YTD],[Measures].[Units Prior YTD Var %],    
                            -----------------------------------
                        [Measures].[On Prem 13 Per TY] ,
                        [Measures].[On Prem 13 Per LY],
                        [Measures].[On Prem 13 Var %],
                        [Measures].[Rol Year Per TY$],
                        [Measures].[Rol Year $ Var %],                              
                      
                        [Measures].[Agent 9L Eq Cases MAT],[Measures].[Agent 9L Eq Cases MAT Pct], 
                        [Measures].[Agent Sales Amount MAT],[Measures].[Agent Sales Amount MAT Pct],
                        [Measures].[Agent Units MAT],[Measures].[Agent Units MAT Pct],
                        [Measures].[Lic 9L Eq Cases MAT],[Measures].[Lic Sales Amount MAT],[Measures].[Lic Units MAT],
                        [Measures].[Lic 9L Eq Cases MAT Pct],
                        [Measures].[Lic Sales Amount MAT Pct],
                        [Measures].[Lic Units MAT Pct]
                        } on columns//param

                        , non empty(  [Product].[Master Color].allmembers  ) --on rows
                        having  [Measures].[Average Sale Price] >" + pricefrom + @" and [Measures].[Average Sale Price] <" + priceto + @"
                        on rows
                        from [LCBO Week]
                        WHERE 
                        (
                        @Period,
                        {@Category},
                        {@UnitSize}
                        )
                        ";
        }

        public static string SalesSummaryByCountry(decimal pricefrom, decimal priceto)
        {
            return @"
                        WITH
                        MEMBER [Measures].[Nb_General] as 0
                        MEMBER [Measures].[Nb_Vintages] as 0
                        MEMBER [Measures].[Nb_VintageEssential] as 0
                        //MEMBER [Measures].[9L Eq Cases CP TY] AS [Measures].[9L Eq Cases], FORMAT_STRING = '#,##0;(#,##0),0'
	                    //MEMBER [Measures].[9L Eq Cases CP LY] AS (ParallelPeriod([Date].[Promotional Period].CurrentMember.Level, 13,[Date].[Promotional Period].CurrentMember),[Measures].[9L Eq Cases]), FORMAT_STRING = '#,##0;(#,##0),0'
	                    MEMBER [Measures].[9L Eq Cases CP LY Var %] AS IIF(Round([9L Eq Cases CP LY],0) = 0, null, (Round([9L Eq Cases CP TY],0) - Round([Measures].[9L Eq Cases CP LY],0)) / Round([Measures].[9L Eq Cases CP LY],0)),format_string='Percent',visible = 1
	                   
	                    //MEMBER [Measures].[9L Eq Cases MAT] AS SUM({[Date].[Promotional Period].currentmember.lag(12):[Date].[Promotional Period].currentmember},[Measures].[9L Eq Cases]), FORMAT_STRING = '#,##0;(#,##0),0'
                        //MEMBER [Measures].[9L Eq Cases Prior MAT] AS SUM({[Date].[Promotional Period].currentmember.lag(25):[Date].[Promotional Period].currentmember.lag(13)}, [Measures].[9L Eq Cases]), FORMAT_STRING = '#,##0;(#,##0),0'
                        //MEMBER [Measures].[9L Eq Cases Prior MAT Var %] AS IIF(round([Measures].[9L Eq Cases Prior MAT],0) = 0, null, (round([Measures].[9L Eq Cases MAT],0) - round([Measures].[9L Eq Cases Prior MAT],0)) / round([Measures].[9L Eq Cases Prior MAT],0)),format_string='Percent',visible = 1

	                    //MEMBER [Measures].[9L Eq Cases YTD] AS SUM(PeriodsToDate([Date].[Promotional].[Year],[Date].[Promotional].currentmember),[Measures].[9L Eq Cases]), FORMAT_STRING = '#,##0;(#,##0),0'
	                    //MEMBER [Measures].[9L Eq Cases Prior YTD] AS SUM(PeriodstoDate([Date].[Promotional].[Year], ParallelPeriod([Date].[Promotional].[Year], 1,[Date].[Promotional].currentmember)),[Measures].[9L Eq Cases]), FORMAT_STRING = '#,##0;(#,##0),0'
	                    MEMBER [Measures].[9L Eq Cases Prior YTD Var %] AS IIF(Round([Measures].[9L Eq Cases Prior YTD],0) = 0, null, (Round([Measures].[9L Eq Cases YTD],0) - Round([Measures].[9L Eq Cases Prior YTD],0)) / Round([Measures].[9L Eq Cases Prior YTD],0)),format_string='Percent',visible = 1

                        MEMBER [Measures].[On Prem 13 Per TY] AS SUM({[Date].[Promotional Period].currentmember.lag(12):[Date].[Promotional Period].currentmember},[Measures].[Units])
                        MEMBER [Measures].[On Prem 13 Per LY] AS SUM({[Date].[Promotional Period].currentmember.lag(25):[Date].[Promotional Period].currentmember.lag(13)}, [Measures].[Units])
                        MEMBER [Measures].[On Prem 13 Var %] AS IIF([Measures].[On Prem 13 Per TY] = 0, null, ([Measures].[On Prem 13 Per TY] - [Measures].[On Prem 13 Per LY]) / [Measures].[On Prem 13 Per TY]),format_string='Percent',visible = 1

                        MEMBER [Measures].[Rol Year Per TY$] AS SUM({[Date].[Promotional Period].currentmember.lag(12):[Date].[Promotional Period].currentmember}, [Measures].[Sales Amount])
                        MEMBER [Measures].[Rol Year $ Var %] AS IIF([Measures].[Rol Year Per TY$] = 0, null, ([Measures].[Rol Year Per TY$]-SUM({[Date].[Promotional Period].currentmember.lag(25):[Date].[Promotional Period].currentmember.lag(13)},[Measures].[Sales Amount])) / [Measures].[Rol Year Per TY$]), format_string='Percent',visible = 1

                        MEMBER [Measures].[Agent 9L Eq Cases MAT] as ([Measures].[9L Eq Cases MAT], [Account].[Channel].&[OFF TRADE]  )
                        MEMBER [Measures].[Lic 9L Eq Cases MAT] as ([Measures].[9L Eq Cases MAT], [Account].[Channel].&[ON TRADE]  )  
                        MEMBER [Measures].[Agent 9L Eq Cases MAT Pct] as ([Measures].[9L Eq Cases Prior MAT Var %] , [Account].[Channel].&[OFF TRADE]  )
                        MEMBER [Measures].[Lic 9L Eq Cases MAT Pct] as ([Measures].[9L Eq Cases Prior MAT Var %], [Account].[Channel].&[ON TRADE]  ) 
                        MEMBER [Measures].[Agent Sales Amount MAT] as ([Measures].[Sales Amount MAT] , [Account].[Channel].&[OFF TRADE]  )
                        MEMBER [Measures].[Lic Sales Amount MAT] as ( [Measures].[Sales Amount MAT], [Account].[Channel].&[ON TRADE]  )
                        MEMBER [Measures].[Agent Sales Amount MAT Pct] as ( [Measures].[Sales Amount Prior MAT Var %] , [Account].[Channel].&[OFF TRADE]  )
                        MEMBER [Measures].[Lic Sales Amount MAT Pct] as (  [Measures].[Sales Amount Prior MAT Var %], [Account].[Channel].&[ON TRADE]  )
                        MEMBER [Measures].[Agent Units MAT] as ( [Measures].[Units MAT] , [Account].[Channel].&[OFF TRADE]  )
                        MEMBER [Measures].[Lic Units MAT] as ( [Measures].[Units MAT] , [Account].[Channel].&[ON TRADE]  )    
                        MEMBER [Measures].[Agent Units MAT Pct] as ( [Measures].[Units Prior MAT Var %]  , [Account].[Channel].&[OFF TRADE]  )
                        MEMBER [Measures].[Lic Units MAT Pct] as (  [Measures].[Units Prior MAT Var %] , [Account].[Channel].&[ON TRADE]  )         

                        SELECT 
                        {
                            -----TY&LY----------------------
                        [Measures].[9L Eq Cases CP TY],[Measures].[9L Eq Cases CP LY],[Measures].[9L Eq Cases CP LY Var %],
                        [Measures].[Sales Amount CP TY],[Measures].[Sales Amount CP LY],[Measures].[Sales Amount CP LY Var %],
                        [Measures].[Units CP TY] ,[Measures].[Units CP LY],[Measures].[Units CP LY Var %],
                            -----6TY&LY----------------------
                        [Measures].[9L Eq Cases P6P],[Measures].[9L Eq Cases Prior P6P],[Measures].[9L Eq Cases Prior P6P Var %],
                        [Measures].[Sales Amount P6P],[Measures].[Sales Amount Prior P6P],[Measures].[Sales Amount Prior P6P Var %],
                        [Measures].[Units P6P],[Measures].[Units Prior P6P],[Measures].[Units Prior P6P Var %],
                            ----------1year-----------------
                        //[Measures].[9L Eq Cases MAT] ,[Measures].[9L Eq Cases Prior MAT],[Measures].[9L Eq Cases Prior MAT Var %],
                        //[Measures].[Sales Amount MAT],[Measures].[Sales Amount Prior MAT],[Measures].[Sales Amount Prior MAT Var %],
                        //[Measures].[Units MAT],[Measures].[Units Prior MAT],[Measures].[Units Prior MAT Var %],
                            ----------ytd----------------------
                        [Measures].[9L Eq Cases YTD],[Measures].[9L Eq Cases Prior YTD],[Measures].[9L Eq Cases Prior YTD Var %],
	                    [Measures].[Sales Amount YTD],[Measures].[Sales Amount Prior YTD],[Measures].[Sales Amount Prior YTD Var %],
                        [Measures].[Units YTD],[Measures].[Units Prior YTD],[Measures].[Units Prior YTD Var %],    
                            -----------------------------------
                        [Measures].[On Prem 13 Per TY] ,
                        [Measures].[On Prem 13 Per LY],
                        [Measures].[On Prem 13 Var %],
                        [Measures].[Rol Year Per TY$],
                        [Measures].[Rol Year $ Var %],                                             
                        [Measures].[Agent 9L Eq Cases MAT],[Measures].[Agent 9L Eq Cases MAT Pct], [Measures].[Agent Sales Amount MAT],[Measures].[Agent Sales Amount MAT Pct],[Measures].[Agent Units MAT],[Measures].[Agent Units MAT Pct],
                        [Measures].[Lic 9L Eq Cases MAT],[Measures].[Lic Sales Amount MAT],[Measures].[Lic Units MAT],
                        [Measures].[Lic 9L Eq Cases MAT Pct],
                        [Measures].[Lic Sales Amount MAT Pct],
                        [Measures].[Lic Units MAT Pct]
                        } on columns//param                                                

                        ,non empty(  [Product].[Master Country].allmembers  ) --on rows
                        having  [Measures].[Average Sale Price] >" + pricefrom + @" and [Measures].[Average Sale Price] <" + priceto + @"
                        on rows
                        from [LCBO Week]
                        WHERE 
                        (
                        @Period,
                        {@Category},
                        {@UnitSize}
                        )
                        ";
        }

        public static string SalesSummaryByVarietal(decimal pricefrom, decimal priceto)
        {
            return @"
                        WITH
                        MEMBER [Measures].[Nb_General] as 0
                        MEMBER [Measures].[Nb_Vintages] as 0
                        MEMBER [Measures].[Nb_VintageEssential] as 0
                        //MEMBER [Measures].[9L Eq Cases CP TY] AS [Measures].[9L Eq Cases], FORMAT_STRING = '#,##0;(#,##0),0'
	                    //MEMBER [Measures].[9L Eq Cases CP LY] AS (ParallelPeriod([Date].[Promotional Period].CurrentMember.Level, 13,[Date].[Promotional Period].CurrentMember),[Measures].[9L Eq Cases]), FORMAT_STRING = '#,##0;(#,##0),0'
	                    MEMBER [Measures].[9L Eq Cases CP LY Var %] AS IIF(Round([9L Eq Cases CP LY],0) = 0, null, (Round([9L Eq Cases CP TY],0) - Round([Measures].[9L Eq Cases CP LY],0)) / Round([Measures].[9L Eq Cases CP LY],0)),format_string='Percent',visible = 1

	                    //MEMBER [Measures].[9L Eq Cases MAT] AS SUM({[Date].[Promotional Period].currentmember.lag(12):[Date].[Promotional Period].currentmember},[Measures].[9L Eq Cases]), FORMAT_STRING = '#,##0;(#,##0),0'
                        //MEMBER [Measures].[9L Eq Cases Prior MAT] AS SUM({[Date].[Promotional Period].currentmember.lag(25):[Date].[Promotional Period].currentmember.lag(13)}, [Measures].[9L Eq Cases]), FORMAT_STRING = '#,##0;(#,##0),0'
                        //MEMBER [Measures].[9L Eq Cases Prior MAT Var %] AS IIF(round([Measures].[9L Eq Cases Prior MAT],0) = 0, null, (round([Measures].[9L Eq Cases MAT],0) - round([Measures].[9L Eq Cases Prior MAT],0)) / round([Measures].[9L Eq Cases Prior MAT],0)),format_string='Percent',visible = 1

	                    //MEMBER [Measures].[9L Eq Cases YTD] AS SUM(PeriodsToDate([Date].[Promotional].[Year],[Date].[Promotional].currentmember),[Measures].[9L Eq Cases]), FORMAT_STRING = '#,##0;(#,##0),0'
	                    //MEMBER [Measures].[9L Eq Cases Prior YTD] AS SUM(PeriodstoDate([Date].[Promotional].[Year], ParallelPeriod([Date].[Promotional].[Year], 1,[Date].[Promotional].currentmember)),[Measures].[9L Eq Cases]), FORMAT_STRING = '#,##0;(#,##0),0'
	                    MEMBER [Measures].[9L Eq Cases Prior YTD Var %] AS IIF(Round([Measures].[9L Eq Cases Prior YTD],0) = 0, null, (Round([Measures].[9L Eq Cases YTD],0) - Round([Measures].[9L Eq Cases Prior YTD],0)) / Round([Measures].[9L Eq Cases Prior YTD],0)),format_string='Percent',visible = 1

                        MEMBER [Measures].[On Prem 13 Per TY] AS SUM({[Date].[Promotional Period].currentmember.lag(12):[Date].[Promotional Period].currentmember},[Measures].[Units])
                        MEMBER [Measures].[On Prem 13 Per LY] AS SUM({[Date].[Promotional Period].currentmember.lag(25):[Date].[Promotional Period].currentmember.lag(13)}, [Measures].[Units])
                        MEMBER [Measures].[On Prem 13 Var %] AS IIF([Measures].[On Prem 13 Per TY] = 0, null, ([Measures].[On Prem 13 Per TY] - [Measures].[On Prem 13 Per LY]) / [Measures].[On Prem 13 Per TY]),format_string='Percent',visible = 1

                        MEMBER [Measures].[Rol Year Per TY$] AS SUM({[Date].[Promotional Period].currentmember.lag(12):[Date].[Promotional Period].currentmember}, [Measures].[Sales Amount])
                        MEMBER [Measures].[Rol Year $ Var %] AS IIF([Measures].[Rol Year Per TY$] = 0, null, ([Measures].[Rol Year Per TY$]-SUM({[Date].[Promotional Period].currentmember.lag(25):[Date].[Promotional Period].currentmember.lag(13)},[Measures].[Sales Amount])) / [Measures].[Rol Year Per TY$]), format_string='Percent',visible = 1

                        MEMBER [Measures].[Agent 9L Eq Cases MAT] as ([Measures].[9L Eq Cases MAT], [Account].[Channel].&[OFF TRADE]  )
                        MEMBER [Measures].[Lic 9L Eq Cases MAT] as ([Measures].[9L Eq Cases MAT], [Account].[Channel].&[ON TRADE]  )  
                        MEMBER [Measures].[Agent 9L Eq Cases MAT Pct] as ([Measures].[9L Eq Cases Prior MAT Var %] , [Account].[Channel].&[OFF TRADE]  )
                        MEMBER [Measures].[Lic 9L Eq Cases MAT Pct] as ([Measures].[9L Eq Cases Prior MAT Var %], [Account].[Channel].&[ON TRADE]  ) 
                        MEMBER [Measures].[Agent Sales Amount MAT] as ([Measures].[Sales Amount MAT] , [Account].[Channel].&[OFF TRADE]  )
                        MEMBER [Measures].[Lic Sales Amount MAT] as ( [Measures].[Sales Amount MAT], [Account].[Channel].&[ON TRADE]  )
                        MEMBER [Measures].[Agent Sales Amount MAT Pct] as ( [Measures].[Sales Amount Prior MAT Var %] , [Account].[Channel].&[OFF TRADE]  )
                        MEMBER [Measures].[Lic Sales Amount MAT Pct] as (  [Measures].[Sales Amount Prior MAT Var %], [Account].[Channel].&[ON TRADE]  )
                        MEMBER [Measures].[Agent Units MAT] as ( [Measures].[Units MAT] , [Account].[Channel].&[OFF TRADE]  )
                        MEMBER [Measures].[Lic Units MAT] as ( [Measures].[Units MAT] , [Account].[Channel].&[ON TRADE]  )    
                        MEMBER [Measures].[Agent Units MAT Pct] as ( [Measures].[Units Prior MAT Var %]  , [Account].[Channel].&[OFF TRADE]  )
                        MEMBER [Measures].[Lic Units MAT Pct] as (  [Measures].[Units Prior MAT Var %] , [Account].[Channel].&[ON TRADE]  )         

                        SELECT 
                        {
                            -----TY&LY----------------------
                        [Measures].[9L Eq Cases CP TY],[Measures].[9L Eq Cases CP LY],[Measures].[9L Eq Cases CP LY Var %],
                        [Measures].[Sales Amount CP TY],[Measures].[Sales Amount CP LY],[Measures].[Sales Amount CP LY Var %],
                        [Measures].[Units CP TY] ,[Measures].[Units CP LY],[Measures].[Units CP LY Var %],
                            -----6TY&LY----------------------
                        [Measures].[9L Eq Cases P6P],[Measures].[9L Eq Cases Prior P6P],[Measures].[9L Eq Cases Prior P6P Var %],
                        [Measures].[Sales Amount P6P],[Measures].[Sales Amount Prior P6P],[Measures].[Sales Amount Prior P6P Var %],
                        [Measures].[Units P6P],[Measures].[Units Prior P6P],[Measures].[Units Prior P6P Var %],
                            ----------1year-----------------
                        //[Measures].[9L Eq Cases MAT] ,[Measures].[9L Eq Cases Prior MAT],[Measures].[9L Eq Cases Prior MAT Var %],
                        //[Measures].[Sales Amount MAT],[Measures].[Sales Amount Prior MAT],[Measures].[Sales Amount Prior MAT Var %],
                        //[Measures].[Units MAT],[Measures].[Units Prior MAT],[Measures].[Units Prior MAT Var %],
                            ----------ytd----------------------
                        [Measures].[9L Eq Cases YTD],[Measures].[9L Eq Cases Prior YTD],[Measures].[9L Eq Cases Prior YTD Var %],
	                    [Measures].[Sales Amount YTD],[Measures].[Sales Amount Prior YTD],[Measures].[Sales Amount Prior YTD Var %],
                        [Measures].[Units YTD],[Measures].[Units Prior YTD],[Measures].[Units Prior YTD Var %],    
                            -----------------------------------
                        [Measures].[On Prem 13 Per TY] ,
                        [Measures].[On Prem 13 Per LY],
                        [Measures].[On Prem 13 Var %],
                        [Measures].[Rol Year Per TY$],
                        [Measures].[Rol Year $ Var %],                                             
                        [Measures].[Agent 9L Eq Cases MAT],[Measures].[Agent 9L Eq Cases MAT Pct], [Measures].[Agent Sales Amount MAT],[Measures].[Agent Sales Amount MAT Pct],[Measures].[Agent Units MAT],[Measures].[Agent Units MAT Pct],
                        [Measures].[Lic 9L Eq Cases MAT],[Measures].[Lic Sales Amount MAT],[Measures].[Lic Units MAT],
                        [Measures].[Lic 9L Eq Cases MAT Pct],
                        [Measures].[Lic Sales Amount MAT Pct],
                        [Measures].[Lic Units MAT Pct]
                        } on columns//param
                        ,non empty(  [Product].[Master Varietal].allmembers  ) --on rows
                        having  [Measures].[Average Sale Price] >" + pricefrom + @" and [Measures].[Average Sale Price] <" + priceto + @"
                        on rows
                        from [LCBO Week]
                        WHERE 
                        (
                        @Period,
                        {@Category},
                        {@UnitSize}
                        )

                        ";
        }

        public static string SalesSummaryByMyCategory(decimal pricefrom, decimal priceto)
        {
            return @"
                        WITH
                        MEMBER [Measures].[Nb_General] as 0
                        MEMBER [Measures].[Nb_Vintages] as 0
                        MEMBER [Measures].[Nb_VintageEssential] as 0
                        //MEMBER [Measures].[9L Eq Cases CP TY] AS [Measures].[9L Eq Cases], FORMAT_STRING = '#,##0;(#,##0),0'
	                    //MEMBER [Measures].[9L Eq Cases CP LY] AS (ParallelPeriod([Date].[Promotional Period].CurrentMember.Level, 13,[Date].[Promotional Period].CurrentMember),[Measures].[9L Eq Cases]), FORMAT_STRING = '#,##0;(#,##0),0'
	                    MEMBER [Measures].[9L Eq Cases CP LY Var %] AS IIF(Round([9L Eq Cases CP LY],0) = 0, null, (Round([9L Eq Cases CP TY],0) - Round([Measures].[9L Eq Cases CP LY],0)) / Round([Measures].[9L Eq Cases CP LY],0)),format_string='Percent',visible = 1

	                    //MEMBER [Measures].[9L Eq Cases MAT] AS SUM({[Date].[Promotional Period].currentmember.lag(12):[Date].[Promotional Period].currentmember},[Measures].[9L Eq Cases]), FORMAT_STRING = '#,##0;(#,##0),0'
                        //MEMBER [Measures].[9L Eq Cases Prior MAT] AS SUM({[Date].[Promotional Period].currentmember.lag(25):[Date].[Promotional Period].currentmember.lag(13)}, [Measures].[9L Eq Cases]), FORMAT_STRING = '#,##0;(#,##0),0'
                        //MEMBER [Measures].[9L Eq Cases Prior MAT Var %] AS IIF(round([Measures].[9L Eq Cases Prior MAT],0) = 0, null, (round([Measures].[9L Eq Cases MAT],0) - round([Measures].[9L Eq Cases Prior MAT],0)) / round([Measures].[9L Eq Cases Prior MAT],0)),format_string='Percent',visible = 1

	                    //MEMBER [Measures].[9L Eq Cases YTD] AS SUM(PeriodsToDate([Date].[Promotional].[Year],[Date].[Promotional].currentmember),[Measures].[9L Eq Cases]), FORMAT_STRING = '#,##0;(#,##0),0'
	                    //MEMBER [Measures].[9L Eq Cases Prior YTD] AS SUM(PeriodstoDate([Date].[Promotional].[Year], ParallelPeriod([Date].[Promotional].[Year], 1,[Date].[Promotional].currentmember)),[Measures].[9L Eq Cases]), FORMAT_STRING = '#,##0;(#,##0),0'
	                    MEMBER [Measures].[9L Eq Cases Prior YTD Var %] AS IIF(Round([Measures].[9L Eq Cases Prior YTD],0) = 0, null, (Round([Measures].[9L Eq Cases YTD],0) - Round([Measures].[9L Eq Cases Prior YTD],0)) / Round([Measures].[9L Eq Cases Prior YTD],0)),format_string='Percent',visible = 1

                        MEMBER [Measures].[On Prem 13 Per TY] AS SUM({[Date].[Promotional Period].currentmember.lag(12):[Date].[Promotional Period].currentmember},[Measures].[Units])
                        MEMBER [Measures].[On Prem 13 Per LY] AS SUM({[Date].[Promotional Period].currentmember.lag(25):[Date].[Promotional Period].currentmember.lag(13)}, [Measures].[Units])
                        MEMBER [Measures].[On Prem 13 Var %] AS IIF([Measures].[On Prem 13 Per TY] = 0, null, ([Measures].[On Prem 13 Per TY] - [Measures].[On Prem 13 Per LY]) / [Measures].[On Prem 13 Per TY]),format_string='Percent',visible = 1

                        MEMBER [Measures].[Rol Year Per TY$] AS SUM({[Date].[Promotional Period].currentmember.lag(12):[Date].[Promotional Period].currentmember}, [Measures].[Sales Amount])
                        MEMBER [Measures].[Rol Year $ Var %] AS IIF([Measures].[Rol Year Per TY$] = 0, null, ([Measures].[Rol Year Per TY$]-SUM({[Date].[Promotional Period].currentmember.lag(25):[Date].[Promotional Period].currentmember.lag(13)},[Measures].[Sales Amount])) / [Measures].[Rol Year Per TY$]), format_string='Percent',visible = 1

                        MEMBER [Measures].[Agent 9L Eq Cases MAT] as ([Measures].[9L Eq Cases MAT], [Account].[Channel].&[OFF TRADE]  )
                        MEMBER [Measures].[Lic 9L Eq Cases MAT] as ([Measures].[9L Eq Cases MAT], [Account].[Channel].&[ON TRADE]  )  
                        MEMBER [Measures].[Agent 9L Eq Cases MAT Pct] as ([Measures].[9L Eq Cases Prior MAT Var %] , [Account].[Channel].&[OFF TRADE]  )
                        MEMBER [Measures].[Lic 9L Eq Cases MAT Pct] as ([Measures].[9L Eq Cases Prior MAT Var %], [Account].[Channel].&[ON TRADE]  ) 
                        MEMBER [Measures].[Agent Sales Amount MAT] as ([Measures].[Sales Amount MAT] , [Account].[Channel].&[OFF TRADE]  )
                        MEMBER [Measures].[Lic Sales Amount MAT] as ( [Measures].[Sales Amount MAT], [Account].[Channel].&[ON TRADE]  )
                        MEMBER [Measures].[Agent Sales Amount MAT Pct] as ( [Measures].[Sales Amount Prior MAT Var %] , [Account].[Channel].&[OFF TRADE]  )
                        MEMBER [Measures].[Lic Sales Amount MAT Pct] as (  [Measures].[Sales Amount Prior MAT Var %], [Account].[Channel].&[ON TRADE]  )
                        MEMBER [Measures].[Agent Units MAT] as ( [Measures].[Units MAT] , [Account].[Channel].&[OFF TRADE]  )
                        MEMBER [Measures].[Lic Units MAT] as ( [Measures].[Units MAT] , [Account].[Channel].&[ON TRADE]  )    
                        MEMBER [Measures].[Agent Units MAT Pct] as ( [Measures].[Units Prior MAT Var %]  , [Account].[Channel].&[OFF TRADE]  )
                        MEMBER [Measures].[Lic Units MAT Pct] as (  [Measures].[Units Prior MAT Var %] , [Account].[Channel].&[ON TRADE]  )         

                        SELECT 
                        {
                            -----TY&LY----------------------
                        [Measures].[9L Eq Cases CP TY],[Measures].[9L Eq Cases CP LY],[Measures].[9L Eq Cases CP LY Var %],
                        [Measures].[Sales Amount CP TY],[Measures].[Sales Amount CP LY],[Measures].[Sales Amount CP LY Var %],
                        [Measures].[Units CP TY] ,[Measures].[Units CP LY],[Measures].[Units CP LY Var %],
                            -----6TY&LY----------------------
                        [Measures].[9L Eq Cases P6P],[Measures].[9L Eq Cases Prior P6P],[Measures].[9L Eq Cases Prior P6P Var %],
                        [Measures].[Sales Amount P6P],[Measures].[Sales Amount Prior P6P],[Measures].[Sales Amount Prior P6P Var %],
                        [Measures].[Units P6P],[Measures].[Units Prior P6P],[Measures].[Units Prior P6P Var %],
                            ----------1year-----------------
                        //[Measures].[9L Eq Cases MAT] ,[Measures].[9L Eq Cases Prior MAT],[Measures].[9L Eq Cases Prior MAT Var %],
                        //[Measures].[Sales Amount MAT],[Measures].[Sales Amount Prior MAT],[Measures].[Sales Amount Prior MAT Var %],
                        //[Measures].[Units MAT],[Measures].[Units Prior MAT],[Measures].[Units Prior MAT Var %],
                            ----------ytd----------------------
                        [Measures].[9L Eq Cases YTD],[Measures].[9L Eq Cases Prior YTD],[Measures].[9L Eq Cases Prior YTD Var %],
	                    [Measures].[Sales Amount YTD],[Measures].[Sales Amount Prior YTD],[Measures].[Sales Amount Prior YTD Var %],
                        [Measures].[Units YTD],[Measures].[Units Prior YTD],[Measures].[Units Prior YTD Var %],    
                            -----------------------------------
                        [Measures].[On Prem 13 Per TY] ,
                        [Measures].[On Prem 13 Per LY],
                        [Measures].[On Prem 13 Var %],
                        [Measures].[Rol Year Per TY$],
                        [Measures].[Rol Year $ Var %],                                             
                        [Measures].[Agent 9L Eq Cases MAT],[Measures].[Agent 9L Eq Cases MAT Pct], [Measures].[Agent Sales Amount MAT],[Measures].[Agent Sales Amount MAT Pct],[Measures].[Agent Units MAT],[Measures].[Agent Units MAT Pct],
                        [Measures].[Lic 9L Eq Cases MAT],[Measures].[Lic Sales Amount MAT],[Measures].[Lic Units MAT],
                        [Measures].[Lic 9L Eq Cases MAT Pct],
                        [Measures].[Lic Sales Amount MAT Pct],
                        [Measures].[Lic Units MAT Pct]
                        } on columns//param
                        ,non empty(  [Product].[My Category].allmembers  ) --on rows
                        having  [Measures].[Average Sale Price] >" + pricefrom + @" and [Measures].[Average Sale Price] <" + priceto + @"
                        on rows
                        from [LCBO Week]
                        WHERE 
                        (
                        @Period,
                        {@Category},
                        {@UnitSize}
                        )
                        ";
        }

        public static string SalesSummaryByPriceBand(decimal pricefrom, decimal priceto)
        {
            return @"
                        WITH
                        MEMBER [Measures].[Nb_General] as 0
                        MEMBER [Measures].[Nb_Vintages] as 0
                        MEMBER [Measures].[Nb_VintageEssential] as 0
                        //MEMBER [Measures].[9L Eq Cases CP TY] AS [Measures].[9L Eq Cases], FORMAT_STRING = '#,##0;(#,##0),0'
	                    //MEMBER [Measures].[9L Eq Cases CP LY] AS (ParallelPeriod([Date].[Promotional Period].CurrentMember.Level, 13,[Date].[Promotional Period].CurrentMember),[Measures].[9L Eq Cases]), FORMAT_STRING = '#,##0;(#,##0),0'
	                    MEMBER [Measures].[9L Eq Cases CP LY Var %] AS IIF(Round([9L Eq Cases CP LY],0) = 0, null, (Round([9L Eq Cases CP TY],0) - Round([Measures].[9L Eq Cases CP LY],0)) / Round([Measures].[9L Eq Cases CP LY],0)),format_string='Percent',visible = 1

	                    //MEMBER [Measures].[9L Eq Cases MAT] AS SUM({[Date].[Promotional Period].currentmember.lag(12):[Date].[Promotional Period].currentmember},[Measures].[9L Eq Cases]), FORMAT_STRING = '#,##0;(#,##0),0'
                        //MEMBER [Measures].[9L Eq Cases Prior MAT] AS SUM({[Date].[Promotional Period].currentmember.lag(25):[Date].[Promotional Period].currentmember.lag(13)}, [Measures].[9L Eq Cases]), FORMAT_STRING = '#,##0;(#,##0),0'
                        //MEMBER [Measures].[9L Eq Cases Prior MAT Var %] AS IIF(round([Measures].[9L Eq Cases Prior MAT],0) = 0, null, (round([Measures].[9L Eq Cases MAT],0) - round([Measures].[9L Eq Cases Prior MAT],0)) / round([Measures].[9L Eq Cases Prior MAT],0)),format_string='Percent',visible = 1

	                    //MEMBER [Measures].[9L Eq Cases YTD] AS SUM(PeriodsToDate([Date].[Promotional].[Year],[Date].[Promotional].currentmember),[Measures].[9L Eq Cases]), FORMAT_STRING = '#,##0;(#,##0),0'
	                    //MEMBER [Measures].[9L Eq Cases Prior YTD] AS SUM(PeriodstoDate([Date].[Promotional].[Year], ParallelPeriod([Date].[Promotional].[Year], 1,[Date].[Promotional].currentmember)),[Measures].[9L Eq Cases]), FORMAT_STRING = '#,##0;(#,##0),0'
	                    MEMBER [Measures].[9L Eq Cases Prior YTD Var %] AS IIF(Round([Measures].[9L Eq Cases Prior YTD],0) = 0, null, (Round([Measures].[9L Eq Cases YTD],0) - Round([Measures].[9L Eq Cases Prior YTD],0)) / Round([Measures].[9L Eq Cases Prior YTD],0)),format_string='Percent',visible = 1

                        MEMBER [Measures].[On Prem 13 Per TY] AS SUM({[Date].[Promotional Period].currentmember.lag(12):[Date].[Promotional Period].currentmember},[Measures].[Units])
                        MEMBER [Measures].[On Prem 13 Per LY] AS SUM({[Date].[Promotional Period].currentmember.lag(25):[Date].[Promotional Period].currentmember.lag(13)}, [Measures].[Units])
                        MEMBER [Measures].[On Prem 13 Var %] AS IIF([Measures].[On Prem 13 Per TY] = 0, null, ([Measures].[On Prem 13 Per TY] - [Measures].[On Prem 13 Per LY]) / [Measures].[On Prem 13 Per TY]),format_string='Percent',visible = 1

                        MEMBER [Measures].[Rol Year Per TY$] AS SUM({[Date].[Promotional Period].currentmember.lag(12):[Date].[Promotional Period].currentmember}, [Measures].[Sales Amount])
                        MEMBER [Measures].[Rol Year $ Var %] AS IIF([Measures].[Rol Year Per TY$] = 0, null, ([Measures].[Rol Year Per TY$]-SUM({[Date].[Promotional Period].currentmember.lag(25):[Date].[Promotional Period].currentmember.lag(13)},[Measures].[Sales Amount])) / [Measures].[Rol Year Per TY$]), format_string='Percent',visible = 1

                        MEMBER [Measures].[Agent 9L Eq Cases MAT] as ([Measures].[9L Eq Cases MAT], [Account].[Channel].&[OFF TRADE]  )
                        MEMBER [Measures].[Lic 9L Eq Cases MAT] as ([Measures].[9L Eq Cases MAT], [Account].[Channel].&[ON TRADE]  )  
                        MEMBER [Measures].[Agent 9L Eq Cases MAT Pct] as ([Measures].[9L Eq Cases Prior MAT Var %] , [Account].[Channel].&[OFF TRADE]  )
                        MEMBER [Measures].[Lic 9L Eq Cases MAT Pct] as ([Measures].[9L Eq Cases Prior MAT Var %], [Account].[Channel].&[ON TRADE]  ) 
                        MEMBER [Measures].[Agent Sales Amount MAT] as ([Measures].[Sales Amount MAT] , [Account].[Channel].&[OFF TRADE]  )
                        MEMBER [Measures].[Lic Sales Amount MAT] as ( [Measures].[Sales Amount MAT], [Account].[Channel].&[ON TRADE]  )
                        MEMBER [Measures].[Agent Sales Amount MAT Pct] as ( [Measures].[Sales Amount Prior MAT Var %] , [Account].[Channel].&[OFF TRADE]  )
                        MEMBER [Measures].[Lic Sales Amount MAT Pct] as (  [Measures].[Sales Amount Prior MAT Var %], [Account].[Channel].&[ON TRADE]  )
                        MEMBER [Measures].[Agent Units MAT] as ( [Measures].[Units MAT] , [Account].[Channel].&[OFF TRADE]  )
                        MEMBER [Measures].[Lic Units MAT] as ( [Measures].[Units MAT] , [Account].[Channel].&[ON TRADE]  )    
                        MEMBER [Measures].[Agent Units MAT Pct] as ( [Measures].[Units Prior MAT Var %]  , [Account].[Channel].&[OFF TRADE]  )
                        MEMBER [Measures].[Lic Units MAT Pct] as (  [Measures].[Units Prior MAT Var %] , [Account].[Channel].&[ON TRADE]  )         

                        SELECT 
                        {
                            -----TY&LY----------------------
                        [Measures].[9L Eq Cases CP TY],[Measures].[9L Eq Cases CP LY],[Measures].[9L Eq Cases CP LY Var %],
                        [Measures].[Sales Amount CP TY],[Measures].[Sales Amount CP LY],[Measures].[Sales Amount CP LY Var %],
                        [Measures].[Units CP TY] ,[Measures].[Units CP LY],[Measures].[Units CP LY Var %],
                            -----6TY&LY----------------------
                        [Measures].[9L Eq Cases P6P],[Measures].[9L Eq Cases Prior P6P],[Measures].[9L Eq Cases Prior P6P Var %],
                        [Measures].[Sales Amount P6P],[Measures].[Sales Amount Prior P6P],[Measures].[Sales Amount Prior P6P Var %],
                        [Measures].[Units P6P],[Measures].[Units Prior P6P],[Measures].[Units Prior P6P Var %],
                            ----------1year-----------------
                        //[Measures].[9L Eq Cases MAT] ,[Measures].[9L Eq Cases Prior MAT],[Measures].[9L Eq Cases Prior MAT Var %],
                        //[Measures].[Sales Amount MAT],[Measures].[Sales Amount Prior MAT],[Measures].[Sales Amount Prior MAT Var %],
                        //[Measures].[Units MAT],[Measures].[Units Prior MAT],[Measures].[Units Prior MAT Var %],
                            ----------ytd----------------------
                        [Measures].[9L Eq Cases YTD],[Measures].[9L Eq Cases Prior YTD],[Measures].[9L Eq Cases Prior YTD Var %],
	                    [Measures].[Sales Amount YTD],[Measures].[Sales Amount Prior YTD],[Measures].[Sales Amount Prior YTD Var %],
                        [Measures].[Units YTD],[Measures].[Units Prior YTD],[Measures].[Units Prior YTD Var %],    
                            -----------------------------------
                        [Measures].[On Prem 13 Per TY] ,
                        [Measures].[On Prem 13 Per LY],
                        [Measures].[On Prem 13 Var %],
                        [Measures].[Rol Year Per TY$],
                        [Measures].[Rol Year $ Var %],                                             
                        [Measures].[Agent 9L Eq Cases MAT],[Measures].[Agent 9L Eq Cases MAT Pct], [Measures].[Agent Sales Amount MAT],[Measures].[Agent Sales Amount MAT Pct],[Measures].[Agent Units MAT],[Measures].[Agent Units MAT Pct],
                        [Measures].[Lic 9L Eq Cases MAT],[Measures].[Lic Sales Amount MAT],[Measures].[Lic Units MAT],
                        [Measures].[Lic 9L Eq Cases MAT Pct],
                        [Measures].[Lic Sales Amount MAT Pct],
                        [Measures].[Lic Units MAT Pct]
                        } on columns//param
                        ,non empty( [Product Price Band].[Product Price Band].allmembers  ) --on rows
                        having  [Measures].[Average Sale Price] >" + pricefrom + @" and [Measures].[Average Sale Price] <" + priceto + @"
                        on rows
                        from [LCBO Week]
                        WHERE 
                        (
                        @Period,
                        {@Category},
                        {@UnitSize}
                        )
                        ";
        }

        public static string SalesSummaryByWeek(int segment, int group)
        {
            //Color
            if (segment == 1)
            {
                //9L Cases
                if (group == 0)
                {
                    return @"
                        select 
                        non empty( [Date].[Promotional Week].[Promotional Week] ) on columns
                        ,non empty(  [Product].[Master Color].[Master Color]  ) on rows
                        from [LCBO Week]
                        where (
                        [Measures].[9L Eq Cases CP TY]
                        ,@Period
                        ,{@UnitSize}
                        ,{@Category}
                        )
                        ";
                    //Sales Amount
                }
                else if (group == 1)
                {
                    return @"
                        select 
                        non empty( [Date].[Promotional Week].[Promotional Week] ) on columns
                        ,non empty(  [Product].[Master Color].[Master Color]  ) on rows
                        from [LCBO Week]
                        where (
                        [Measures].[Sales Amount CP TY]
                        ,@Period
                        ,{@UnitSize}
                        ,{@Category}
                        )
                        ";
                    //Units
                }
                else
                {
                    return @"
                        select 
                        non empty( [Date].[Promotional Week].[Promotional Week] ) on columns
                        ,non empty(  [Product].[Master Color].[Master Color]  ) on rows
                        from [LCBO Week]
                        where (
                        [Measures].[Units CP TY]
                        ,@Period
                        ,{@UnitSize}
                        ,{@Category}
                        )
                        ";
                }
                //Country
            }
            else if (segment == 2)
            {
                //9L Cases
                if (group == 0)
                {
                    return @"
                        select 
                        non empty( [Date].[Promotional Week].[Promotional Week] ) on columns
                        ,non empty(  [Product].[Master Country].[Master Country]  ) on rows
                        from [LCBO Week]
                        where (
                        [Measures].[9L Eq Cases CP TY]
                        ,@Period
                        ,{@UnitSize}
                        ,{@Category}
                        )
                        ";
                    //Sales Amount
                }
                else if (group == 1)
                {
                    return @"
                        select 
                        non empty( [Date].[Promotional Week].[Promotional Week] ) on columns
                        ,non empty(  [Product].[Master Country].[Master Country]  ) on rows
                        from [LCBO Week]
                        where (
                        [Measures].[Sales Amount CP TY]
                        ,@Period
                        ,{@UnitSize}
                        ,{@Category}
                        )
                        ";
                    //Units
                }
                else
                {
                    return @"
                        select 
                        non empty( [Date].[Promotional Week].[Promotional Week] ) on columns
                        ,non empty(  [Product].[Master Country].[Master Country]  ) on rows
                        from [LCBO Week]
                        where (
                        [Measures].[Units CP TY]
                        ,@Period
                        ,{@UnitSize}
                        ,{@Category}
                        )
                        ";
                }
                //Varietal
            }
            else if (segment == 3)
            {
                //9L Cases
                if (group == 0)
                {
                    return @"
                        select 
                        non empty( [Date].[Promotional Week].[Promotional Week] ) on columns
                        ,non empty(  [Product].[Master Varietal].[Master Varietal]  ) on rows
                        from [LCBO Week]
                        where (
                        [Measures].[9L Eq Cases CP TY]
                        ,@Period
                        ,{@UnitSize}
                        ,{@Category}
                        )
                        ";
                    //Sales Amount
                }
                else if (group == 1)
                {
                    return @"
                        select 
                        non empty( [Date].[Promotional Week].[Promotional Week] ) on columns
                        ,non empty(  [Product].[Master Varietal].[Master Varietal]  ) on rows
                        from [LCBO Week]
                        where (
                        [Measures].[Sales Amount CP TY]
                        ,@Period
                        ,{@UnitSize}
                        ,{@Category}
                        )
                        ";
                    //Units
                }
                else
                {
                    return @"
                        select 
                        non empty( [Date].[Promotional Week].[Promotional Week] ) on columns
                        ,non empty(  [Product].[Master Varietal].[Master Varietal]  ) on rows
                        from [LCBO Week]
                        where (
                        [Measures].[Units CP TY]
                        ,@Period
                        ,{@UnitSize}
                        ,{@Category}
                        )
                        ";
                }
            }
            else if (segment == 4)//My Category
            {
                //9L Cases
                if (group == 0)
                {
                    return @"
                        select 
                        non empty( [Date].[Promotional Week].[Promotional Week] ) on columns
                        ,non empty(  [Product].[My Category].[My Category]  ) on rows
                        from [LCBO Week]
                        where (
                        [Measures].[9L Eq Cases CP TY]
                        ,@Period
                        ,{@UnitSize}
                        ,{@Category}
                        )
                        ";
                    //Sales Amount
                }
                else if (group == 1)
                {
                    return @"
                        select 
                        non empty( [Date].[Promotional Week].[Promotional Week] ) on columns
                        ,non empty(  [Product].[My Category].[My Category]  ) on rows
                        from [LCBO Week]
                        where (
                        [Measures].[Sales Amount CP TY]
                        ,@Period
                        ,{@UnitSize}
                        ,{@Category}
                        )
                        ";
                    //Units
                }
                else
                {
                    return @"
                        select 
                        non empty( [Date].[Promotional Week].[Promotional Week] ) on columns
                        ,non empty(  [Product].[My Category].[My Category]  ) on rows
                        from [LCBO Week]
                        where (
                        [Measures].[Units CP TY]
                        ,@Period
                        ,{@UnitSize}
                        ,{@Category}
                        )
                        ";
                }
            }
            else //Price Band
            {
                //9L Cases
                if (group == 0)
                {
                    return @"
                    select 
                    non empty( [Date].[Promotional Week].[Promotional Week] ) on columns
                    ,non empty( [Product Price Band].[Product Price Band].[Product Price Band] ) on rows
                    from [LCBO Week]
                    where (
                    [Measures].[9L Eq Cases CP TY]
                    ,@Period
                    ,{@UnitSize}
                    ,{@Category}
                    )
                    ";
                    //Sales Amount
                }
                else if (group == 1)
                {
                    return @"
                    select 
                    non empty( [Date].[Promotional Week].[Promotional Week] ) on columns
                    ,non empty( [Product Price Band].[Product Price Band].[Product Price Band]  ) on rows
                    from [LCBO Week]
                    where (
                    [Measures].[Sales Amount CP TY]
                    ,@Period
                    ,{@UnitSize}
                    ,{@Category}
                    )
                    ";
                    //Units
                }
                else
                {
                    return @"
                    select 
                    non empty( [Date].[Promotional Week].[Promotional Week] ) on columns
                    ,non empty( [Product Price Band].[Product Price Band].[Product Price Band] ) on rows
                    from [LCBO Week]
                    where (
                    [Measures].[Units CP TY]
                    ,@Period
                    ,{@UnitSize}
                    ,{@Category}
                    )
                    ";
                }
            }

        }


        public static string SalesSummaryColorProductByWeek(string color, int group)
        {
            //9L Cases
            if (group == 0)
            {
                return @"
                    select 
                    non empty( [Date].[Promotional Week].[Promotional Week] ) on columns
                    ,non empty( [Product].[Product Name].[Product Name] ) on rows
                    from [LCBO Week]
                    where (
                    [Measures].[9L Eq Cases CP TY]
                    ,@Period
                    ,{@UnitSize}
                    ,@Color
                    ,{@Category}
                )";

            }
            else if (group == 1)  //Sales Amount
            {
                return @"
                    select 
                    non empty( [Date].[Promotional Week].[Promotional Week] ) on columns
                    ,non empty( [Product].[Product Name].[Product Name] ) on rows
                    from [LCBO Week]
                    where (
                    [Measures].[Sales Amount CP TY]
                    ,@Period
                    ,{@UnitSize}
                    ,@Color
                    ,{@Category}
                )";

            }
            else   //Units
            {
                return @"
                    select 
                    non empty( [Date].[Promotional Week].[Promotional Week] ) on columns
                    ,non empty( [Product].[Product Name].[Product Name] ) on rows
                    from [LCBO Week]
                    where (
                    [Measures].[Units CP TY]
                    ,@Period
                    ,{@UnitSize}
                    ,@Color
                    ,{@Category}
                )";
            }
        }


        public static string SalesSummaryColorCountryProductByWeek(string color, string country, int group)
        {
            //9L Cases
            if (group == 0)
            {
                return @"
                    select 
                    non empty( [Date].[Promotional Week].[Promotional Week] ) on columns
                    ,non empty( [Product].[Product Name].[Product Name] ) on rows
                    from [LCBO Week]
                    where (
                    [Measures].[9L Eq Cases CP TY]
                    ,@Period
                    ,{@UnitSize}
                    ,@Color
                    ,@Country
                    ,{@Category}
                )";

            }
            else if (group == 1)  //Sales Amount
            {
                return @"
                    select 
                    non empty( [Date].[Promotional Week].[Promotional Week] ) on columns
                    ,non empty( [Product].[Product Name].[Product Name] ) on rows
                    from [LCBO Week]
                    where (
                    [Measures].[Sales Amount CP TY]
                    ,@Period
                    ,{@UnitSize}
                    ,@Color
                    ,@Country
                    ,{@Category}
                )";

            }
            else   //Units
            {
                return @"
                    select 
                    non empty( [Date].[Promotional Week].[Promotional Week] ) on columns
                    ,non empty( [Product].[Product Name].[Product Name] ) on rows
                    from [LCBO Week]
                    where (
                    [Measures].[Units CP TY]
                    ,@Period
                    ,{@UnitSize}
                    ,@Color
                    ,@Country
                    ,{@Category}
                )";
            }
        }

        public static string SalesSummaryPriceBandMyCategoryProductByWeek(string priceband, string mycategory, int group)
        {
            //9L Cases
            if (group == 0)
            {
                return @"
                    select 
                    non empty( [Date].[Promotional Week].[Promotional Week] ) on columns
                    ,non empty( [Product].[Product Name].[Product Name] ) on rows
                    from [LCBO Week]
                    where (
                    [Measures].[9L Eq Cases CP TY]
                    ,@Period
                    ,{@UnitSize}
                    ,@PriceBand
                    ,@MyCategory
                    ,{@Category}
                )";

            }
            else if (group == 1)  //Sales Amount
            {
                return @"
                    select 
                    non empty( [Date].[Promotional Week].[Promotional Week] ) on columns
                    ,non empty( [Product].[Product Name].[Product Name] ) on rows
                    from [LCBO Week]
                    where (
                    [Measures].[Sales Amount CP TY]
                    ,@Period
                    ,{@UnitSize}
                    ,@PriceBand
                    ,@MyCategory
                    ,{@Category}
                )";

            }
            else   //Units
            {
                return @"
                    select 
                    non empty( [Date].[Promotional Week].[Promotional Week] ) on columns
                    ,non empty( [Product].[Product Name].[Product Name] ) on rows
                    from [LCBO Week]
                    where (
                    [Measures].[Units CP TY]
                    ,@Period
                    ,{@UnitSize}
                    ,@PriceBand
                    ,@MyCategory
                    ,{@Category}
                )";
            }
        }

        public static string SalesSummaryCountryColorProductByWeek(string country, string color, int group)
        {
            //9L Cases
            if (group == 0)
            {
                return @"
                    select 
                    non empty( [Date].[Promotional Week].[Promotional Week] ) on columns
                    ,non empty( [Product].[Product Name].[Product Name] ) on rows
                    from [LCBO Week]
                    where (
                    [Measures].[9L Eq Cases CP TY]
                    ,@Period
                    ,{@UnitSize}
                    ,@Color
                    ,@Country
                    ,{@Category}
                )";

            }
            else if (group == 1)  //Sales Amount
            {
                return @"
                    select 
                    non empty( [Date].[Promotional Week].[Promotional Week] ) on columns
                    ,non empty( [Product].[Product Name].[Product Name] ) on rows
                    from [LCBO Week]
                    where (
                    [Measures].[Sales Amount CP TY]
                    ,@Period
                    ,{@UnitSize}
                    ,@Color
                    ,@Country
                    ,{@Category}
                )";

            }
            else   //Units
            {
                return @"
                    select 
                    non empty( [Date].[Promotional Week].[Promotional Week] ) on columns
                    ,non empty( [Product].[Product Name].[Product Name] ) on rows
                    from [LCBO Week]
                    where (
                    [Measures].[Units CP TY]
                    ,@Period
                    ,{@UnitSize}
                    ,@Color
                    ,@Country
                    ,{@Category}
                )";
            }
        }

        public static string SalesSummaryColorCountryByWeek(string color, int group)
        {
            //9L Cases
            if (group == 0)
            {
                return @"
                    select 
                    non empty( [Date].[Promotional Week].[Promotional Week] ) on columns
                    ,non empty( [Product].[Master Country].[Master Country] ) on rows
                    from [LCBO Week]
                    where (
                    [Measures].[9L Eq Cases CP TY]
                    ,@Period
                    ,{@UnitSize}
                    ,@Color
                    ,{@Category}
                )";

            }
            else if (group == 1)  //Sales Amount
            {
                return @"
                    select 
                    non empty( [Date].[Promotional Week].[Promotional Week] ) on columns
                    ,non empty( [Product].[Master Country].[Master Country] ) on rows
                    from [LCBO Week]
                    where (
                    [Measures].[Sales Amount CP TY]
                    ,@Period
                    ,{@UnitSize}
                    ,@Color
                    ,{@Category}
                )";

            }
            else   //Units
            {
                return @"
                    select 
                    non empty( [Date].[Promotional Week].[Promotional Week] ) on columns
                    ,non empty( [Product].[Master Country].[Master Country] ) on rows
                    from [LCBO Week]
                    where (
                    [Measures].[Units CP TY]
                    ,@Period
                    ,{@UnitSize}
                    ,@Color
                    ,{@Category}
                )";
            }
        }

        public static string SalesSummaryVarietalProductByWeek(string varietal, int group)
        {
            //9L Cases
            if (group == 0)
            {
                return @"
                    select 
                    non empty( [Date].[Promotional Week].[Promotional Week] ) on columns
                    ,non empty( [Product].[Master Product].[Master Product] ) on rows
                    from [LCBO Week]
                    where (
                    [Measures].[9L Eq Cases CP TY]
                    ,@Period
                    ,{@UnitSize}
                    ,{@Varietal}
                    ,{@Category}
                )";

            }
            else if (group == 1)  //Sales Amount
            {
                return @"
                    select 
                    non empty( [Date].[Promotional Week].[Promotional Week] ) on columns
                    ,non empty( [Product].[Master Product].[Master Product] ) on rows
                    from [LCBO Week]
                    where (
                    [Measures].[Sales Amount CP TY]
                    ,@Period
                    ,{@UnitSize}
                    ,{@Varietal}
                    ,{@Category}
                )";

            }
            else   //Units
            {
                return @"
                    select 
                    non empty( [Date].[Promotional Week].[Promotional Week] ) on columns
                    ,non empty( [Product].[Master Product].[Master Product] ) on rows
                    from [LCBO Week]
                    where (
                    [Measures].[Units CP TY]
                    ,@Period
                    ,{@UnitSize}
                    ,{@Varietal}
                    ,{@Category}
                )";
            }
        }

        public static string SalesSummaryCountryColorByWeek(string country, int group)
        {
            //9L Cases
            if (group == 0)
            {
                return @"
                    select 
                    non empty( [Date].[Promotional Week].[Promotional Week] ) on columns
                    ,non empty( [Product].[Master Color].[Master Color] ) on rows
                    from [LCBO Week]
                    where (
                    [Measures].[9L Eq Cases CP TY]
                    ,@Period
                    ,{@UnitSize}
                    ,@Country
                    ,{@Category}
                )";

            }
            else if (group == 1)  //Sales Amount
            {
                return @"
                    select 
                    non empty( [Date].[Promotional Week].[Promotional Week] ) on columns
                    ,non empty( [Product].[Master Color].[Master Color] ) on rows
                    from [LCBO Week]
                    where (
                    [Measures].[Sales Amount CP TY]
                    ,@Period
                    ,{@UnitSize}
                    ,@Country
                    ,{@Category}
                )";

            }
            else   //Units
            {
                return @"
                    select 
                    non empty( [Date].[Promotional Week].[Promotional Week] ) on columns
                    ,non empty( [Product].[Master Color].[Master Color] ) on rows
                    from [LCBO Week]
                    where (
                    [Measures].[Units CP TY]
                    ,@Period
                    ,{@UnitSize}
                    ,@Country
                    ,{@Category}
                )";
            }
        }

        public static string SalesSummaryMyCategoryCountryByWeek(string mycategory, int group)
        {
            //9L Cases
            if (group == 0)
            {
                return @"
                    select 
                    non empty( [Date].[Promotional Week].[Promotional Week] ) on columns
                    ,non empty( [Product].[Master Country].[Master Country] ) on rows
                    from [LCBO Week]
                    where (
                    [Measures].[9L Eq Cases CP TY]
                    ,@Period
                    ,{@UnitSize}
                    ,@MyCategory
                    ,{@Category}
                )";

            }
            else if (group == 1)  //Sales Amount
            {
                return @"
                    select 
                    non empty( [Date].[Promotional Week].[Promotional Week] ) on columns
                    ,non empty( [Product].[Master Country].[Master Country] ) on rows
                    from [LCBO Week]
                    where (
                    [Measures].[Sales Amount CP TY]
                    ,@Period
                    ,{@UnitSize}
                    ,@MyCategory
                    ,{@Category}
                )";

            }
            else   //Units
            {
                return @"
                    select 
                    non empty( [Date].[Promotional Week].[Promotional Week] ) on columns
                    ,non empty( [Product].[Master Country].[Master Country] ) on rows
                    from [LCBO Week]
                    where (
                    [Measures].[Units CP TY]
                    ,@Period
                    ,{@UnitSize}
                    ,@MyCategory
                    ,{@Category}
                )";
            }
        }

        public static string SalesSummaryMyCategoryCountryProductByWeek(string mycategory, string country, int group)
        {
            //9L Cases
            if (group == 0)
            {
                return @"
                    select 
                    non empty( [Date].[Promotional Week].[Promotional Week] ) on columns
                    ,non empty( [Product].[Product Name].[Product Name] ) on rows
                    from [LCBO Week]
                    where (
                    [Measures].[9L Eq Cases CP TY]
                    ,@Period
                    ,{@UnitSize}
                    ,@MyCategory
                    ,@Country
                    ,{@Category}
                )";

            }
            else if (group == 1)  //Sales Amount
            {
                return @"
                    select 
                    non empty( [Date].[Promotional Week].[Promotional Week] ) on columns
                    ,non empty( [Product].[Product Name].[Product Name] ) on rows
                    from [LCBO Week]
                    where (
                    [Measures].[Sales Amount CP TY]
                    ,@Period
                    ,{@UnitSize}
                    ,@MyCategory
                    ,@Country
                    ,{@Category}
                )";

            }
            else   //Units
            {
                return @"
                    select 
                    non empty( [Date].[Promotional Week].[Promotional Week] ) on columns
                    ,non empty( [Product].[Product Name].[Product Name] ) on rows
                    from [LCBO Week]
                    where (
                    [Measures].[Units CP TY]
                    ,@Period
                    ,{@UnitSize}
                    ,@MyCategory
                    ,@Country
                    ,{@Category}
                )";
            }
        }

        public static string SalesSummaryPriceBandCategoryByWeek(string priceband, int group)
        {
            //9L Cases
            if (group == 0)
            {
                return @"
                    select 
                    non empty( [Date].[Promotional Week].[Promotional Week] ) on columns
                    ,non empty( [Product].[My Category] ) on rows
                    from [LCBO Week]
                    where (
                    [Measures].[9L Eq Cases CP TY]
                    ,@Period
                    ,{@UnitSize}
                    ,@PriceBand
                    ,{@Category}
                )";

            }
            else if (group == 1)  //Sales Amount
            {
                return @"
                    select 
                    non empty( [Date].[Promotional Week].[Promotional Week] ) on columns
                    ,non empty( [Product].[My Category] ) on rows
                    from [LCBO Week]
                    where (
                    [Measures].[Sales Amount CP TY]
                    ,@Period
                    ,{@UnitSize}
                    ,@PriceBand
                    ,{@Category}
                )";

            }
            else   //Units
            {
                return @"
                    select 
                    non empty( [Date].[Promotional Week].[Promotional Week] ) on columns
                    ,non empty( [Product].[My Category] ) on rows
                    from [LCBO Week]
                    where (
                    [Measures].[Units CP TY]
                    ,@Period
                    ,{@UnitSize}
                    ,@PriceBand
                    ,{@Category}
                )";
            }
        }

        public static string SalesSummaryColorCountry(decimal pricefrom, decimal priceto)
        {
            return @"
                       WITH
                MEMBER [Measures].[Nb_General] as 0
                MEMBER [Measures].[Nb_Vintages] as 0
                MEMBER [Measures].[Nb_VintageEssential] as 0
                //MEMBER [Measures].[9L Eq Cases CP TY] AS [Measures].[9L Eq Cases], FORMAT_STRING = '#,##0;(#,##0),0'
				//MEMBER [Measures].[9L Eq Cases CP LY] AS (ParallelPeriod([Date].[Promotional Period].CurrentMember.Level, 13,[Date].[Promotional Period].CurrentMember),[Measures].[9L Eq Cases]), FORMAT_STRING = '#,##0;(#,##0),0'
				MEMBER [Measures].[9L Eq Cases CP LY Var %] AS IIF(Round([9L Eq Cases CP LY],0) = 0, null, (Round([9L Eq Cases CP TY],0) - Round([Measures].[9L Eq Cases CP LY],0)) / Round([Measures].[9L Eq Cases CP LY],0)),format_string='Percent',visible = 1

				//MEMBER [Measures].[9L Eq Cases MAT] AS SUM({[Date].[Promotional Period].currentmember.lag(12):[Date].[Promotional Period].currentmember},[Measures].[9L Eq Cases]), FORMAT_STRING = '#,##0;(#,##0),0'
				//MEMBER [Measures].[9L Eq Cases Prior MAT] AS SUM({[Date].[Promotional Period].currentmember.lag(25):[Date].[Promotional Period].currentmember.lag(13)}, [Measures].[9L Eq Cases]), FORMAT_STRING = '#,##0;(#,##0),0'
				//MEMBER [Measures].[9L Eq Cases Prior MAT Var %] AS IIF(round([Measures].[9L Eq Cases Prior MAT],0) = 0, null, (round([Measures].[9L Eq Cases MAT],0) - round([Measures].[9L Eq Cases Prior MAT],0)) / round([Measures].[9L Eq Cases Prior MAT],0)),format_string='Percent',visible = 1

				//MEMBER [Measures].[9L Eq Cases YTD] AS SUM(PeriodsToDate([Date].[Promotional].[Year],[Date].[Promotional].currentmember),[Measures].[9L Eq Cases]), FORMAT_STRING = '#,##0;(#,##0),0'
				//MEMBER [Measures].[9L Eq Cases Prior YTD] AS SUM(PeriodstoDate([Date].[Promotional].[Year], ParallelPeriod([Date].[Promotional].[Year], 1,[Date].[Promotional].currentmember)),[Measures].[9L Eq Cases]), FORMAT_STRING = '#,##0;(#,##0),0'
				MEMBER [Measures].[9L Eq Cases Prior YTD Var %] AS IIF(Round([Measures].[9L Eq Cases Prior YTD],0) = 0, null, (Round([Measures].[9L Eq Cases YTD],0) - Round([Measures].[9L Eq Cases Prior YTD],0)) / Round([Measures].[9L Eq Cases Prior YTD],0)),format_string='Percent',visible = 1

				MEMBER [Measures].[On Prem 13 Per TY] AS SUM({[Date].[Promotional Period].currentmember.lag(12):[Date].[Promotional Period].currentmember},[Measures].[Units])
				MEMBER [Measures].[On Prem 13 Per LY] AS SUM({[Date].[Promotional Period].currentmember.lag(25):[Date].[Promotional Period].currentmember.lag(13)}, [Measures].[Units])
				MEMBER [Measures].[On Prem 13 Var %] AS IIF([Measures].[On Prem 13 Per TY] = 0, null, ([Measures].[On Prem 13 Per TY] - [Measures].[On Prem 13 Per LY]) / [Measures].[On Prem 13 Per TY]),format_string='Percent',visible = 1

				MEMBER [Measures].[Rol Year Per TY$] AS SUM({[Date].[Promotional Period].currentmember.lag(12):[Date].[Promotional Period].currentmember}, [Measures].[Sales Amount])
				MEMBER [Measures].[Rol Year $ Var %] AS IIF([Measures].[Rol Year Per TY$] = 0, null, ([Measures].[Rol Year Per TY$]-SUM({[Date].[Promotional Period].currentmember.lag(25):[Date].[Promotional Period].currentmember.lag(13)},[Measures].[Sales Amount])) / [Measures].[Rol Year Per TY$]), format_string='Percent',visible = 1

                MEMBER [Measures].[Agent 9L Eq Cases MAT] as ([Measures].[9L Eq Cases MAT], [Account].[Channel].&[OFF TRADE]  )
                MEMBER [Measures].[Lic 9L Eq Cases MAT] as ([Measures].[9L Eq Cases MAT], [Account].[Channel].&[ON TRADE]  )  
                MEMBER [Measures].[Agent 9L Eq Cases MAT Pct] as ([Measures].[9L Eq Cases Prior MAT Var %] , [Account].[Channel].&[OFF TRADE]  )
                MEMBER [Measures].[Lic 9L Eq Cases MAT Pct] as ([Measures].[9L Eq Cases Prior MAT Var %], [Account].[Channel].&[ON TRADE]  ) 
                MEMBER [Measures].[Agent Sales Amount MAT] as ([Measures].[Sales Amount MAT] , [Account].[Channel].&[OFF TRADE]  )
                MEMBER [Measures].[Lic Sales Amount MAT] as ( [Measures].[Sales Amount MAT], [Account].[Channel].&[ON TRADE]  )
                MEMBER [Measures].[Agent Sales Amount MAT Pct] as ( [Measures].[Sales Amount Prior MAT Var %] , [Account].[Channel].&[OFF TRADE]  )
                MEMBER [Measures].[Lic Sales Amount MAT Pct] as (  [Measures].[Sales Amount Prior MAT Var %], [Account].[Channel].&[ON TRADE]  )
                MEMBER [Measures].[Agent Units MAT] as ( [Measures].[Units MAT] , [Account].[Channel].&[OFF TRADE]  )
                MEMBER [Measures].[Lic Units MAT] as ( [Measures].[Units MAT] , [Account].[Channel].&[ON TRADE]  )    
                MEMBER [Measures].[Agent Units MAT Pct] as ( [Measures].[Units Prior MAT Var %]  , [Account].[Channel].&[OFF TRADE]  )
                MEMBER [Measures].[Lic Units MAT Pct] as (  [Measures].[Units Prior MAT Var %] , [Account].[Channel].&[ON TRADE]  )         
                SELECT 
               non empty ( {          
                [Measures].[Nb_General],
                [Measures].[Nb_Vintages],
                [Measures].[Nb_VintageEssential] ,                    
                    -----TY&LY----------------------
                [Measures].[9L Eq Cases CP TY],[Measures].[9L Eq Cases CP LY],[Measures].[9L Eq Cases CP LY Var %],
                [Measures].[Sales Amount CP TY],[Measures].[Sales Amount CP LY],[Measures].[Sales Amount CP LY Var %],
                [Measures].[Units CP TY] ,[Measures].[Units CP LY],[Measures].[Units CP LY Var %],
                    -----6TY&LY----------------------
                [Measures].[9L Eq Cases P6P],[Measures].[9L Eq Cases Prior P6P],[Measures].[9L Eq Cases Prior P6P Var %],
                [Measures].[Sales Amount P6P],[Measures].[Sales Amount Prior P6P],[Measures].[Sales Amount Prior P6P Var %],
                [Measures].[Units P6P],[Measures].[Units Prior P6P],[Measures].[Units Prior P6P Var %],
                ----------1year-----------------
                //[Measures].[9L Eq Cases MAT] ,[Measures].[9L Eq Cases Prior MAT],[Measures].[9L Eq Cases Prior MAT Var %],
                //[Measures].[Sales Amount MAT],[Measures].[Sales Amount Prior MAT],[Measures].[Sales Amount Prior MAT Var %],
                //[Measures].[Units MAT],[Measures].[Units Prior MAT],[Measures].[Units Prior MAT Var %],
                 ----------ytd----------------------
                [Measures].[Sales Amount YTD],[Measures].[Sales Amount Prior YTD],[Measures].[Sales Amount Prior YTD Var %],
               [Measures].[Units YTD],[Measures].[Units Prior YTD],[Measures].[Units Prior YTD Var %],
                [Measures].[9L Eq Cases YTD],[Measures].[9L Eq Cases Prior YTD],[Measures].[9L Eq Cases Prior YTD Var %],
                -----------------------------------
                [Measures].[On Prem 13 Per TY] ,
                [Measures].[On Prem 13 Per LY],
                [Measures].[On Prem 13 Var %],
                [Measures].[Rol Year Per TY$],
                [Measures].[Rol Year $ Var %],                                                         
                [Measures].[Agent 9L Eq Cases MAT],[Measures].[Agent 9L Eq Cases MAT Pct], [Measures].[Agent Sales Amount MAT],[Measures].[Agent Sales Amount MAT Pct],[Measures].[Agent Units MAT],[Measures].[Agent Units MAT Pct],  
                [Measures].[Lic 9L Eq Cases MAT],[Measures].[Lic Sales Amount MAT],[Measures].[Lic Units MAT],[Measures].[Lic 9L Eq Cases MAT Pct],[Measures].[Lic Sales Amount MAT Pct],[Measures].[Lic Units MAT Pct]   
                     }) on columns,
               non empty(
                    [Product].[Master Country].allmembers)
                having  [Measures].[Average Sale Price] >" + pricefrom + @" and [Measures].[Average Sale Price] <" + priceto + @"
                on rows
                FROM[LCBO Week]
                WHERE 
                (
                   @Period,
                  {@ColorName},
                  {@Category},
                  {@UnitSize}
                )
            ";
        }

        public static string SalesSummaryColorCountryProduct(decimal pricefrom, decimal priceto)
        {
            return @"
                       WITH
                MEMBER [Measures].[Nb_General] as 0
                MEMBER [Measures].[Nb_Vintages] as 0
                MEMBER [Measures].[Nb_VintageEssential] as 0
                //MEMBER [Measures].[9L Eq Cases CP TY] AS [Measures].[9L Eq Cases], FORMAT_STRING = '#,##0;(#,##0),0'
				//MEMBER [Measures].[9L Eq Cases CP LY] AS (ParallelPeriod([Date].[Promotional Period].CurrentMember.Level, 13,[Date].[Promotional Period].CurrentMember),[Measures].[9L Eq Cases]), FORMAT_STRING = '#,##0;(#,##0),0'
				MEMBER [Measures].[9L Eq Cases CP LY Var %] AS IIF(Round([9L Eq Cases CP LY],0) = 0, null, (Round([9L Eq Cases CP TY],0) - Round([Measures].[9L Eq Cases CP LY],0)) / Round([Measures].[9L Eq Cases CP LY],0)),format_string='Percent',visible = 1

				//MEMBER [Measures].[9L Eq Cases MAT] AS SUM({[Date].[Promotional Period].currentmember.lag(12):[Date].[Promotional Period].currentmember},[Measures].[9L Eq Cases]), FORMAT_STRING = '#,##0;(#,##0),0'
				//MEMBER [Measures].[9L Eq Cases Prior MAT] AS SUM({[Date].[Promotional Period].currentmember.lag(25):[Date].[Promotional Period].currentmember.lag(13)}, [Measures].[9L Eq Cases]), FORMAT_STRING = '#,##0;(#,##0),0'
				//MEMBER [Measures].[9L Eq Cases Prior MAT Var %] AS IIF(round([Measures].[9L Eq Cases Prior MAT],0) = 0, null, (round([Measures].[9L Eq Cases MAT],0) - round([Measures].[9L Eq Cases Prior MAT],0)) / round([Measures].[9L Eq Cases Prior MAT],0)),format_string='Percent',visible = 1

				//MEMBER [Measures].[9L Eq Cases YTD] AS SUM(PeriodsToDate([Date].[Promotional].[Year],[Date].[Promotional].currentmember),[Measures].[9L Eq Cases]), FORMAT_STRING = '#,##0;(#,##0),0'
				//MEMBER [Measures].[9L Eq Cases Prior YTD] AS SUM(PeriodstoDate([Date].[Promotional].[Year], ParallelPeriod([Date].[Promotional].[Year], 1,[Date].[Promotional].currentmember)),[Measures].[9L Eq Cases]), FORMAT_STRING = '#,##0;(#,##0),0'
				MEMBER [Measures].[9L Eq Cases Prior YTD Var %] AS IIF(Round([Measures].[9L Eq Cases Prior YTD],0) = 0, null, (Round([Measures].[9L Eq Cases YTD],0) - Round([Measures].[9L Eq Cases Prior YTD],0)) / Round([Measures].[9L Eq Cases Prior YTD],0)),format_string='Percent',visible = 1

				MEMBER [Measures].[On Prem 13 Per TY] AS SUM({[Date].[Promotional Period].currentmember.lag(12):[Date].[Promotional Period].currentmember},[Measures].[Units])
				MEMBER [Measures].[On Prem 13 Per LY] AS SUM({[Date].[Promotional Period].currentmember.lag(25):[Date].[Promotional Period].currentmember.lag(13)}, [Measures].[Units])
				MEMBER [Measures].[On Prem 13 Var %] AS IIF([Measures].[On Prem 13 Per TY] = 0, null, ([Measures].[On Prem 13 Per TY] - [Measures].[On Prem 13 Per LY]) / [Measures].[On Prem 13 Per TY]),format_string='Percent',visible = 1

				MEMBER [Measures].[Rol Year Per TY$] AS SUM({[Date].[Promotional Period].currentmember.lag(12):[Date].[Promotional Period].currentmember}, [Measures].[Sales Amount])
				MEMBER [Measures].[Rol Year $ Var %] AS IIF([Measures].[Rol Year Per TY$] = 0, null, ([Measures].[Rol Year Per TY$]-SUM({[Date].[Promotional Period].currentmember.lag(25):[Date].[Promotional Period].currentmember.lag(13)},[Measures].[Sales Amount])) / [Measures].[Rol Year Per TY$]), format_string='Percent',visible = 1

                MEMBER [Measures].[Agent 9L Eq Cases MAT] as ([Measures].[9L Eq Cases MAT], [Account].[Channel].&[OFF TRADE]  )
                MEMBER [Measures].[Lic 9L Eq Cases MAT] as ([Measures].[9L Eq Cases MAT], [Account].[Channel].&[ON TRADE]  )  
                MEMBER [Measures].[Agent 9L Eq Cases MAT Pct] as ([Measures].[9L Eq Cases Prior MAT Var %] , [Account].[Channel].&[OFF TRADE]  )
                MEMBER [Measures].[Lic 9L Eq Cases MAT Pct] as ([Measures].[9L Eq Cases Prior MAT Var %], [Account].[Channel].&[ON TRADE]  ) 
                MEMBER [Measures].[Agent Sales Amount MAT] as ([Measures].[Sales Amount MAT] , [Account].[Channel].&[OFF TRADE]  )
                MEMBER [Measures].[Lic Sales Amount MAT] as ( [Measures].[Sales Amount MAT], [Account].[Channel].&[ON TRADE]  )
                MEMBER [Measures].[Agent Sales Amount MAT Pct] as ( [Measures].[Sales Amount Prior MAT Var %] , [Account].[Channel].&[OFF TRADE]  )
                MEMBER [Measures].[Lic Sales Amount MAT Pct] as (  [Measures].[Sales Amount Prior MAT Var %], [Account].[Channel].&[ON TRADE]  )
                MEMBER [Measures].[Agent Units MAT] as ( [Measures].[Units MAT] , [Account].[Channel].&[OFF TRADE]  )
                MEMBER [Measures].[Lic Units MAT] as ( [Measures].[Units MAT] , [Account].[Channel].&[ON TRADE]  )    
                MEMBER [Measures].[Agent Units MAT Pct] as ( [Measures].[Units Prior MAT Var %]  , [Account].[Channel].&[OFF TRADE]  )
                MEMBER [Measures].[Lic Units MAT Pct] as (  [Measures].[Units Prior MAT Var %] , [Account].[Channel].&[ON TRADE]  )         
                SELECT 
               non empty ( {          
                [Measures].[Nb_General],
                [Measures].[Nb_Vintages],
                [Measures].[Nb_VintageEssential] ,                    
                    -----TY&LY----------------------
                [Measures].[9L Eq Cases CP TY],[Measures].[9L Eq Cases CP LY],[Measures].[9L Eq Cases CP LY Var %],
                [Measures].[Sales Amount CP TY],[Measures].[Sales Amount CP LY],[Measures].[Sales Amount CP LY Var %],
                [Measures].[Units CP TY] ,[Measures].[Units CP LY],[Measures].[Units CP LY Var %],
                    -----6TY&LY----------------------
                [Measures].[9L Eq Cases P6P],[Measures].[9L Eq Cases Prior P6P],[Measures].[9L Eq Cases Prior P6P Var %],
                [Measures].[Sales Amount P6P],[Measures].[Sales Amount Prior P6P],[Measures].[Sales Amount Prior P6P Var %],
                [Measures].[Units P6P],[Measures].[Units Prior P6P],[Measures].[Units Prior P6P Var %],
                ----------1year-----------------
                //[Measures].[9L Eq Cases MAT] ,[Measures].[9L Eq Cases Prior MAT],[Measures].[9L Eq Cases Prior MAT Var %],
                //[Measures].[Sales Amount MAT],[Measures].[Sales Amount Prior MAT],[Measures].[Sales Amount Prior MAT Var %],
                //[Measures].[Units MAT],[Measures].[Units Prior MAT],[Measures].[Units Prior MAT Var %],
                 ----------ytd----------------------
                [Measures].[Sales Amount YTD],[Measures].[Sales Amount Prior YTD],[Measures].[Sales Amount Prior YTD Var %],
                [Measures].[Units YTD],[Measures].[Units Prior YTD],[Measures].[Units Prior YTD Var %],
                [Measures].[9L Eq Cases YTD],[Measures].[9L Eq Cases Prior YTD],[Measures].[9L Eq Cases Prior YTD Var %],
                -----------------------------------
                [Measures].[On Prem 13 Per TY] ,
                [Measures].[On Prem 13 Per LY],
                [Measures].[On Prem 13 Var %],
                [Measures].[Rol Year Per TY$],
                [Measures].[Rol Year $ Var %],                                                         
                [Measures].[Agent 9L Eq Cases MAT],[Measures].[Agent 9L Eq Cases MAT Pct], [Measures].[Agent Sales Amount MAT],[Measures].[Agent Sales Amount MAT Pct],[Measures].[Agent Units MAT],[Measures].[Agent Units MAT Pct],  
                [Measures].[Lic 9L Eq Cases MAT],[Measures].[Lic Sales Amount MAT],[Measures].[Lic Units MAT],[Measures].[Lic 9L Eq Cases MAT Pct],[Measures].[Lic Sales Amount MAT Pct],[Measures].[Lic Units MAT Pct]   
                     }) on columns,
               non empty(
                    [Product].[Master Product].allmembers)
                having  [Measures].[Average Sale Price] >" + pricefrom + @" and [Measures].[Average Sale Price] <" + priceto + @"
                on rows
                FROM[LCBO Week]
                WHERE 
                (
                   @Period,
                  {@ColorName},
                  {@CountryName},
                  {@Category},
                  {@UnitSize}
                )
            ";
        }


        public static string SalesSummaryCountryColorProduct(decimal pricefrom, decimal priceto)
        {
            return @"
                       WITH
                        MEMBER [Measures].[Nb_General] as 0
                        MEMBER [Measures].[Nb_Vintages] as 0
                        MEMBER [Measures].[Nb_VintageEssential] as 0
                        
                        //MEMBER [Measures].[9L Eq Cases CP TY] AS [Measures].[9L Eq Cases], FORMAT_STRING = '#,##0;(#,##0),0'
				        //MEMBER [Measures].[9L Eq Cases CP LY] AS (ParallelPeriod([Date].[Promotional Period].CurrentMember.Level, 13,[Date].[Promotional Period].CurrentMember),[Measures].[9L Eq Cases]), FORMAT_STRING = '#,##0;(#,##0),0'
				        MEMBER [Measures].[9L Eq Cases CP LY Var %] AS IIF(Round([9L Eq Cases CP LY],0) = 0, null, (Round([9L Eq Cases CP TY],0) - Round([Measures].[9L Eq Cases CP LY],0)) / Round([Measures].[9L Eq Cases CP LY],0)),format_string='Percent',visible = 1

				        //MEMBER [Measures].[9L Eq Cases MAT] AS SUM({[Date].[Promotional Period].currentmember.lag(12):[Date].[Promotional Period].currentmember},[Measures].[9L Eq Cases]), FORMAT_STRING = '#,##0;(#,##0),0'
				        //MEMBER [Measures].[9L Eq Cases Prior MAT] AS SUM({[Date].[Promotional Period].currentmember.lag(25):[Date].[Promotional Period].currentmember.lag(13)}, [Measures].[9L Eq Cases]), FORMAT_STRING = '#,##0;(#,##0),0'
				        //MEMBER [Measures].[9L Eq Cases Prior MAT Var %] AS IIF(round([Measures].[9L Eq Cases Prior MAT],0) = 0, null, (round([Measures].[9L Eq Cases MAT],0) - round([Measures].[9L Eq Cases Prior MAT],0)) / round([Measures].[9L Eq Cases Prior MAT],0)),format_string='Percent',visible = 1

				        //MEMBER [Measures].[9L Eq Cases YTD] AS SUM(PeriodsToDate([Date].[Promotional].[Year],[Date].[Promotional].currentmember),[Measures].[9L Eq Cases]), FORMAT_STRING = '#,##0;(#,##0),0'
				        //MEMBER [Measures].[9L Eq Cases Prior YTD] AS SUM(PeriodstoDate([Date].[Promotional].[Year], ParallelPeriod([Date].[Promotional].[Year], 1,[Date].[Promotional].currentmember)),[Measures].[9L Eq Cases]), FORMAT_STRING = '#,##0;(#,##0),0'
				        MEMBER [Measures].[9L Eq Cases Prior YTD Var %] AS IIF(Round([Measures].[9L Eq Cases Prior YTD],0) = 0, null, (Round([Measures].[9L Eq Cases YTD],0) - Round([Measures].[9L Eq Cases Prior YTD],0)) / Round([Measures].[9L Eq Cases Prior YTD],0)),format_string='Percent',visible = 1

				        MEMBER [Measures].[On Prem 13 Per TY] AS SUM({[Date].[Promotional Period].currentmember.lag(12):[Date].[Promotional Period].currentmember},[Measures].[Units])
				        MEMBER [Measures].[On Prem 13 Per LY] AS SUM({[Date].[Promotional Period].currentmember.lag(25):[Date].[Promotional Period].currentmember.lag(13)}, [Measures].[Units])
				        MEMBER [Measures].[On Prem 13 Var %] AS IIF([Measures].[On Prem 13 Per TY] = 0, null, ([Measures].[On Prem 13 Per TY] - [Measures].[On Prem 13 Per LY]) / [Measures].[On Prem 13 Per TY]),format_string='Percent',visible = 1

				        MEMBER [Measures].[Rol Year Per TY$] AS SUM({[Date].[Promotional Period].currentmember.lag(12):[Date].[Promotional Period].currentmember}, [Measures].[Sales Amount])
				        MEMBER [Measures].[Rol Year $ Var %] AS IIF([Measures].[Rol Year Per TY$] = 0, null, ([Measures].[Rol Year Per TY$]-SUM({[Date].[Promotional Period].currentmember.lag(25):[Date].[Promotional Period].currentmember.lag(13)},[Measures].[Sales Amount])) / [Measures].[Rol Year Per TY$]), format_string='Percent',visible = 1


                        MEMBER [Measures].[Agent 9L Eq Cases MAT] as ([Measures].[9L Eq Cases MAT], [Account].[Channel].&[OFF TRADE]  )
                        MEMBER [Measures].[Lic 9L Eq Cases MAT] as ([Measures].[9L Eq Cases MAT], [Account].[Channel].&[ON TRADE]  )  
                        MEMBER [Measures].[Agent 9L Eq Cases MAT Pct] as ([Measures].[9L Eq Cases Prior MAT Var %] , [Account].[Channel].&[OFF TRADE]  )
                        MEMBER [Measures].[Lic 9L Eq Cases MAT Pct] as ([Measures].[9L Eq Cases Prior MAT Var %], [Account].[Channel].&[ON TRADE]  ) 
                        MEMBER [Measures].[Agent Sales Amount MAT] as ([Measures].[Sales Amount MAT] , [Account].[Channel].&[OFF TRADE]  )
                        MEMBER [Measures].[Lic Sales Amount MAT] as ( [Measures].[Sales Amount MAT], [Account].[Channel].&[ON TRADE]  )
                        MEMBER [Measures].[Agent Sales Amount MAT Pct] as ( [Measures].[Sales Amount Prior MAT Var %] , [Account].[Channel].&[OFF TRADE]  )
                        MEMBER [Measures].[Lic Sales Amount MAT Pct] as (  [Measures].[Sales Amount Prior MAT Var %], [Account].[Channel].&[ON TRADE]  )
                        MEMBER [Measures].[Agent Units MAT] as ( [Measures].[Units MAT] , [Account].[Channel].&[OFF TRADE]  )
                        MEMBER [Measures].[Lic Units MAT] as ( [Measures].[Units MAT] , [Account].[Channel].&[ON TRADE]  )    
                        MEMBER [Measures].[Agent Units MAT Pct] as ( [Measures].[Units Prior MAT Var %]  , [Account].[Channel].&[OFF TRADE]  )
                        MEMBER [Measures].[Lic Units MAT Pct] as (  [Measures].[Units Prior MAT Var %] , [Account].[Channel].&[ON TRADE]  )         

                        SELECT 
                        {
                            -----TY&LY----------------------
                        [Measures].[9L Eq Cases CP TY],[Measures].[9L Eq Cases CP LY],[Measures].[9L Eq Cases CP LY Var %],
                        [Measures].[Sales Amount CP TY],[Measures].[Sales Amount CP LY],[Measures].[Sales Amount CP LY Var %],
                        [Measures].[Units CP TY] ,[Measures].[Units CP LY],[Measures].[Units CP LY Var %],
                            -----6TY&LY----------------------
                        [Measures].[9L Eq Cases P6P],[Measures].[9L Eq Cases Prior P6P],[Measures].[9L Eq Cases Prior P6P Var %],
                        [Measures].[Sales Amount P6P],[Measures].[Sales Amount Prior P6P],[Measures].[Sales Amount Prior P6P Var %],
                        [Measures].[Units P6P],[Measures].[Units Prior P6P],[Measures].[Units Prior P6P Var %],
                            ----------1year-----------------
                        //[Measures].[9L Eq Cases MAT] ,[Measures].[9L Eq Cases Prior MAT],[Measures].[9L Eq Cases Prior MAT Var %],
                        //[Measures].[Sales Amount MAT],[Measures].[Sales Amount Prior MAT],[Measures].[Sales Amount Prior MAT Var %],
                        //[Measures].[Units MAT],[Measures].[Units Prior MAT],[Measures].[Units Prior MAT Var %],
                            ----------ytd----------------------
                        [Measures].[Sales Amount YTD],[Measures].[Sales Amount Prior YTD],[Measures].[Sales Amount Prior YTD Var %],
                        [Measures].[Units YTD],[Measures].[Units Prior YTD],[Measures].[Units Prior YTD Var %],
                        [Measures].[9L Eq Cases YTD],[Measures].[9L Eq Cases Prior YTD],[Measures].[9L Eq Cases Prior YTD Var %],
                            -----------------------------------
                        [Measures].[On Prem 13 Per TY] ,
                        [Measures].[On Prem 13 Per LY],
                        [Measures].[On Prem 13 Var %],
                        [Measures].[Rol Year Per TY$],
                        [Measures].[Rol Year $ Var %],                                             
                        [Measures].[Agent 9L Eq Cases MAT],[Measures].[Agent 9L Eq Cases MAT Pct], [Measures].[Agent Sales Amount MAT],[Measures].[Agent Sales Amount MAT Pct],[Measures].[Agent Units MAT],[Measures].[Agent Units MAT Pct],
                        [Measures].[Lic 9L Eq Cases MAT],[Measures].[Lic Sales Amount MAT],[Measures].[Lic Units MAT],
                                   [Measures].[Lic 9L Eq Cases MAT Pct],
                                   [Measures].[Lic Sales Amount MAT Pct],
                                   [Measures].[Lic Units MAT Pct]
                        } on columns//param
                        ,non empty(  [Product].[Master Product].allmembers  ) --on rows
                        having  [Measures].[Average Sale Price] >" + pricefrom + @" and [Measures].[Average Sale Price] <" + priceto + @"
                        on rows
                        from [LCBO Week]
                        WHERE 
                        (
                        @Period,
                        {@Category},
                        {@CountryName},
			            {@Color},
                        {@UnitSize}
                        )

            ";
        }


        public static string SalesSummaryCountryColor(decimal pricefrom, decimal priceto)
        {
            return @"
                       WITH
                        MEMBER [Measures].[Nb_General] as 0
                        MEMBER [Measures].[Nb_Vintages] as 0
                        MEMBER [Measures].[Nb_VintageEssential] as 0

                        //MEMBER [Measures].[9L Eq Cases CP TY] AS [Measures].[9L Eq Cases], FORMAT_STRING = '#,##0;(#,##0),0'
						//MEMBER [Measures].[9L Eq Cases CP LY] AS (ParallelPeriod([Date].[Promotional Period].CurrentMember.Level, 13,[Date].[Promotional Period].CurrentMember),[Measures].[9L Eq Cases]), FORMAT_STRING = '#,##0;(#,##0),0'
						MEMBER [Measures].[9L Eq Cases CP LY Var %] AS IIF(Round([9L Eq Cases CP LY],0) = 0, null, (Round([9L Eq Cases CP TY],0) - Round([Measures].[9L Eq Cases CP LY],0)) / Round([Measures].[9L Eq Cases CP LY],0)),format_string='Percent',visible = 1

						//MEMBER [Measures].[9L Eq Cases MAT] AS SUM({[Date].[Promotional Period].currentmember.lag(12):[Date].[Promotional Period].currentmember},[Measures].[9L Eq Cases]), FORMAT_STRING = '#,##0;(#,##0),0'
						//MEMBER [Measures].[9L Eq Cases Prior MAT] AS SUM({[Date].[Promotional Period].currentmember.lag(25):[Date].[Promotional Period].currentmember.lag(13)}, [Measures].[9L Eq Cases]), FORMAT_STRING = '#,##0;(#,##0),0'
						//MEMBER [Measures].[9L Eq Cases Prior MAT Var %] AS IIF(round([Measures].[9L Eq Cases Prior MAT],0) = 0, null, (round([Measures].[9L Eq Cases MAT],0) - round([Measures].[9L Eq Cases Prior MAT],0)) / round([Measures].[9L Eq Cases Prior MAT],0)),format_string='Percent',visible = 1

						//MEMBER [Measures].[9L Eq Cases YTD] AS SUM(PeriodsToDate([Date].[Promotional].[Year],[Date].[Promotional].currentmember),[Measures].[9L Eq Cases]), FORMAT_STRING = '#,##0;(#,##0),0'
						//MEMBER [Measures].[9L Eq Cases Prior YTD] AS SUM(PeriodstoDate([Date].[Promotional].[Year], ParallelPeriod([Date].[Promotional].[Year], 1,[Date].[Promotional].currentmember)),[Measures].[9L Eq Cases]), FORMAT_STRING = '#,##0;(#,##0),0'
						MEMBER [Measures].[9L Eq Cases Prior YTD Var %] AS IIF(Round([Measures].[9L Eq Cases Prior YTD],0) = 0, null, (Round([Measures].[9L Eq Cases YTD],0) - Round([Measures].[9L Eq Cases Prior YTD],0)) / Round([Measures].[9L Eq Cases Prior YTD],0)),format_string='Percent',visible = 1

						MEMBER [Measures].[On Prem 13 Per TY] AS SUM({[Date].[Promotional Period].currentmember.lag(12):[Date].[Promotional Period].currentmember},[Measures].[Units])
						MEMBER [Measures].[On Prem 13 Per LY] AS SUM({[Date].[Promotional Period].currentmember.lag(25):[Date].[Promotional Period].currentmember.lag(13)}, [Measures].[Units])
						MEMBER [Measures].[On Prem 13 Var %] AS IIF([Measures].[On Prem 13 Per TY] = 0, null, ([Measures].[On Prem 13 Per TY] - [Measures].[On Prem 13 Per LY]) / [Measures].[On Prem 13 Per TY]),format_string='Percent',visible = 1

						MEMBER [Measures].[Rol Year Per TY$] AS SUM({[Date].[Promotional Period].currentmember.lag(12):[Date].[Promotional Period].currentmember}, [Measures].[Sales Amount])
						MEMBER [Measures].[Rol Year $ Var %] AS IIF([Measures].[Rol Year Per TY$] = 0, null, ([Measures].[Rol Year Per TY$]-SUM({[Date].[Promotional Period].currentmember.lag(25):[Date].[Promotional Period].currentmember.lag(13)},[Measures].[Sales Amount])) / [Measures].[Rol Year Per TY$]), format_string='Percent',visible = 1


                        MEMBER [Measures].[Agent 9L Eq Cases MAT] as ([Measures].[9L Eq Cases MAT], [Account].[Channel].&[OFF TRADE]  )
                        MEMBER [Measures].[Lic 9L Eq Cases MAT] as ([Measures].[9L Eq Cases MAT], [Account].[Channel].&[ON TRADE]  )  
                        MEMBER [Measures].[Agent 9L Eq Cases MAT Pct] as ([Measures].[9L Eq Cases Prior MAT Var %] , [Account].[Channel].&[OFF TRADE]  )
                        MEMBER [Measures].[Lic 9L Eq Cases MAT Pct] as ([Measures].[9L Eq Cases Prior MAT Var %], [Account].[Channel].&[ON TRADE]  ) 
                        MEMBER [Measures].[Agent Sales Amount MAT] as ([Measures].[Sales Amount MAT] , [Account].[Channel].&[OFF TRADE]  )
                        MEMBER [Measures].[Lic Sales Amount MAT] as ( [Measures].[Sales Amount MAT], [Account].[Channel].&[ON TRADE]  )
                        MEMBER [Measures].[Agent Sales Amount MAT Pct] as ( [Measures].[Sales Amount Prior MAT Var %] , [Account].[Channel].&[OFF TRADE]  )
                        MEMBER [Measures].[Lic Sales Amount MAT Pct] as (  [Measures].[Sales Amount Prior MAT Var %], [Account].[Channel].&[ON TRADE]  )
                        MEMBER [Measures].[Agent Units MAT] as ( [Measures].[Units MAT] , [Account].[Channel].&[OFF TRADE]  )
                        MEMBER [Measures].[Lic Units MAT] as ( [Measures].[Units MAT] , [Account].[Channel].&[ON TRADE]  )    
                        MEMBER [Measures].[Agent Units MAT Pct] as ( [Measures].[Units Prior MAT Var %]  , [Account].[Channel].&[OFF TRADE]  )
                        MEMBER [Measures].[Lic Units MAT Pct] as (  [Measures].[Units Prior MAT Var %] , [Account].[Channel].&[ON TRADE]  )         

                        SELECT 
                        {
                            -----TY&LY----------------------
                        [Measures].[9L Eq Cases CP TY],[Measures].[9L Eq Cases CP LY],[Measures].[9L Eq Cases CP LY Var %],
                        [Measures].[Sales Amount CP TY],[Measures].[Sales Amount CP LY],[Measures].[Sales Amount CP LY Var %],
                        [Measures].[Units CP TY] ,[Measures].[Units CP LY],[Measures].[Units CP LY Var %],
                            -----6TY&LY----------------------
                        [Measures].[9L Eq Cases P6P],[Measures].[9L Eq Cases Prior P6P],[Measures].[9L Eq Cases Prior P6P Var %],
                        [Measures].[Sales Amount P6P],[Measures].[Sales Amount Prior P6P],[Measures].[Sales Amount Prior P6P Var %],
                        [Measures].[Units P6P],[Measures].[Units Prior P6P],[Measures].[Units Prior P6P Var %],
                            ----------1year-----------------
                        //[Measures].[9L Eq Cases MAT] ,[Measures].[9L Eq Cases Prior MAT],[Measures].[9L Eq Cases Prior MAT Var %],
                        //[Measures].[Sales Amount MAT],[Measures].[Sales Amount Prior MAT],[Measures].[Sales Amount Prior MAT Var %],
                        //[Measures].[Units MAT],[Measures].[Units Prior MAT],[Measures].[Units Prior MAT Var %],
                            ----------ytd----------------------
                        [Measures].[Sales Amount YTD],[Measures].[Sales Amount Prior YTD],[Measures].[Sales Amount Prior YTD Var %],
                        [Measures].[Units YTD],[Measures].[Units Prior YTD],[Measures].[Units Prior YTD Var %],
                        [Measures].[9L Eq Cases YTD],[Measures].[9L Eq Cases Prior YTD],[Measures].[9L Eq Cases Prior YTD Var %],
                            -----------------------------------
                        [Measures].[On Prem 13 Per TY] ,
                        [Measures].[On Prem 13 Per LY],
                        [Measures].[On Prem 13 Var %],
                        [Measures].[Rol Year Per TY$],
                        [Measures].[Rol Year $ Var %],                                             
                        [Measures].[Agent 9L Eq Cases MAT],[Measures].[Agent 9L Eq Cases MAT Pct], [Measures].[Agent Sales Amount MAT],[Measures].[Agent Sales Amount MAT Pct],[Measures].[Agent Units MAT],[Measures].[Agent Units MAT Pct],
                        [Measures].[Lic 9L Eq Cases MAT],[Measures].[Lic Sales Amount MAT],[Measures].[Lic Units MAT],
                                   [Measures].[Lic 9L Eq Cases MAT Pct],
                                   [Measures].[Lic Sales Amount MAT Pct],
                                   [Measures].[Lic Units MAT Pct]
                        } on columns//param
                        ,non empty(  [Product].[Master Color].allmembers  ) --on rows
                        having  [Measures].[Average Sale Price] >" + pricefrom + @" and [Measures].[Average Sale Price] <" + priceto + @"
                        on rows
                        from [LCBO Week]
                        WHERE 
                        (
                        @Period,
                        {@Category},
                        {@CountryName},
                        {@UnitSize}
                        )
            ";
        }

        public static string SalesSummaryVarietalProduct(decimal pricefrom, decimal priceto)
        {
            return @"
                      WITH
                        MEMBER [Measures].[Nb_General] as 0
                        MEMBER [Measures].[Nb_Vintages] as 0
                        MEMBER [Measures].[Nb_VintageEssential] as 0
                        //MEMBER [Measures].[9L Eq Cases CP TY] AS [Measures].[9L Eq Cases], FORMAT_STRING = '#,##0;(#,##0),0'
						//MEMBER [Measures].[9L Eq Cases CP LY] AS (ParallelPeriod([Date].[Promotional Period].CurrentMember.Level, 13,[Date].[Promotional Period].CurrentMember),[Measures].[9L Eq Cases]), FORMAT_STRING = '#,##0;(#,##0),0'
						MEMBER [Measures].[9L Eq Cases CP LY Var %] AS IIF(Round([9L Eq Cases CP LY],0) = 0, null, (Round([9L Eq Cases CP TY],0) - Round([Measures].[9L Eq Cases CP LY],0)) / Round([Measures].[9L Eq Cases CP LY],0)),format_string='Percent',visible = 1

						//MEMBER [Measures].[9L Eq Cases MAT] AS SUM({[Date].[Promotional Period].currentmember.lag(12):[Date].[Promotional Period].currentmember},[Measures].[9L Eq Cases]), FORMAT_STRING = '#,##0;(#,##0),0'
						//MEMBER [Measures].[9L Eq Cases Prior MAT] AS SUM({[Date].[Promotional Period].currentmember.lag(25):[Date].[Promotional Period].currentmember.lag(13)}, [Measures].[9L Eq Cases]), FORMAT_STRING = '#,##0;(#,##0),0'
						//MEMBER [Measures].[9L Eq Cases Prior MAT Var %] AS IIF(round([Measures].[9L Eq Cases Prior MAT],0) = 0, null, (round([Measures].[9L Eq Cases MAT],0) - round([Measures].[9L Eq Cases Prior MAT],0)) / round([Measures].[9L Eq Cases Prior MAT],0)),format_string='Percent',visible = 1

						//MEMBER [Measures].[9L Eq Cases YTD] AS SUM(PeriodsToDate([Date].[Promotional].[Year],[Date].[Promotional].currentmember),[Measures].[9L Eq Cases]), FORMAT_STRING = '#,##0;(#,##0),0'
						//MEMBER [Measures].[9L Eq Cases Prior YTD] AS SUM(PeriodstoDate([Date].[Promotional].[Year], ParallelPeriod([Date].[Promotional].[Year], 1,[Date].[Promotional].currentmember)),[Measures].[9L Eq Cases]), FORMAT_STRING = '#,##0;(#,##0),0'
						MEMBER [Measures].[9L Eq Cases Prior YTD Var %] AS IIF(Round([Measures].[9L Eq Cases Prior YTD],0) = 0, null, (Round([Measures].[9L Eq Cases YTD],0) - Round([Measures].[9L Eq Cases Prior YTD],0)) / Round([Measures].[9L Eq Cases Prior YTD],0)),format_string='Percent',visible = 1

						MEMBER [Measures].[On Prem 13 Per TY] AS SUM({[Date].[Promotional Period].currentmember.lag(12):[Date].[Promotional Period].currentmember},[Measures].[Units])
						MEMBER [Measures].[On Prem 13 Per LY] AS SUM({[Date].[Promotional Period].currentmember.lag(25):[Date].[Promotional Period].currentmember.lag(13)}, [Measures].[Units])
						MEMBER [Measures].[On Prem 13 Var %] AS IIF([Measures].[On Prem 13 Per TY] = 0, null, ([Measures].[On Prem 13 Per TY] - [Measures].[On Prem 13 Per LY]) / [Measures].[On Prem 13 Per TY]),format_string='Percent',visible = 1

						MEMBER [Measures].[Rol Year Per TY$] AS SUM({[Date].[Promotional Period].currentmember.lag(12):[Date].[Promotional Period].currentmember}, [Measures].[Sales Amount])
						MEMBER [Measures].[Rol Year $ Var %] AS IIF([Measures].[Rol Year Per TY$] = 0, null, ([Measures].[Rol Year Per TY$]-SUM({[Date].[Promotional Period].currentmember.lag(25):[Date].[Promotional Period].currentmember.lag(13)},[Measures].[Sales Amount])) / [Measures].[Rol Year Per TY$]), format_string='Percent',visible = 1

                        MEMBER [Measures].[Agent 9L Eq Cases MAT] as ([Measures].[9L Eq Cases MAT], [Account].[Channel].&[OFF TRADE]  )
                        MEMBER [Measures].[Lic 9L Eq Cases MAT] as ([Measures].[9L Eq Cases MAT], [Account].[Channel].&[ON TRADE]  )  
                        MEMBER [Measures].[Agent 9L Eq Cases MAT Pct] as ([Measures].[9L Eq Cases Prior MAT Var %] , [Account].[Channel].&[OFF TRADE]  )
                        MEMBER [Measures].[Lic 9L Eq Cases MAT Pct] as ([Measures].[9L Eq Cases Prior MAT Var %], [Account].[Channel].&[ON TRADE]  ) 
                        MEMBER [Measures].[Agent Sales Amount MAT] as ([Measures].[Sales Amount MAT] , [Account].[Channel].&[OFF TRADE]  )
                        MEMBER [Measures].[Lic Sales Amount MAT] as ( [Measures].[Sales Amount MAT], [Account].[Channel].&[ON TRADE]  )
                        MEMBER [Measures].[Agent Sales Amount MAT Pct] as ( [Measures].[Sales Amount Prior MAT Var %] , [Account].[Channel].&[OFF TRADE]  )
                        MEMBER [Measures].[Lic Sales Amount MAT Pct] as (  [Measures].[Sales Amount Prior MAT Var %], [Account].[Channel].&[ON TRADE]  )
                        MEMBER [Measures].[Agent Units MAT] as ( [Measures].[Units MAT] , [Account].[Channel].&[OFF TRADE]  )
                        MEMBER [Measures].[Lic Units MAT] as ( [Measures].[Units MAT] , [Account].[Channel].&[ON TRADE]  )    
                        MEMBER [Measures].[Agent Units MAT Pct] as ( [Measures].[Units Prior MAT Var %]  , [Account].[Channel].&[OFF TRADE]  )
                        MEMBER [Measures].[Lic Units MAT Pct] as (  [Measures].[Units Prior MAT Var %] , [Account].[Channel].&[ON TRADE]  )         

                        SELECT 
                        {
                            -----TY&LY----------------------
                        [Measures].[9L Eq Cases CP TY],[Measures].[9L Eq Cases CP LY],[Measures].[9L Eq Cases CP LY Var %],
                        [Measures].[Sales Amount CP TY],[Measures].[Sales Amount CP LY],[Measures].[Sales Amount CP LY Var %],
                        [Measures].[Units CP TY] ,[Measures].[Units CP LY],[Measures].[Units CP LY Var %],
                            -----6TY&LY----------------------
                        [Measures].[9L Eq Cases P6P],[Measures].[9L Eq Cases Prior P6P],[Measures].[9L Eq Cases Prior P6P Var %],
                        [Measures].[Sales Amount P6P],[Measures].[Sales Amount Prior P6P],[Measures].[Sales Amount Prior P6P Var %],
                        [Measures].[Units P6P],[Measures].[Units Prior P6P],[Measures].[Units Prior P6P Var %],
                            ----------1year-----------------
                        //[Measures].[9L Eq Cases MAT] ,[Measures].[9L Eq Cases Prior MAT],[Measures].[9L Eq Cases Prior MAT Var %],
                        //[Measures].[Sales Amount MAT],[Measures].[Sales Amount Prior MAT],[Measures].[Sales Amount Prior MAT Var %],
                        //[Measures].[Units MAT],[Measures].[Units Prior MAT],[Measures].[Units Prior MAT Var %],
                            ----------ytd----------------------
                        [Measures].[Sales Amount YTD],[Measures].[Sales Amount Prior YTD],[Measures].[Sales Amount Prior YTD Var %],
                        [Measures].[Units YTD],[Measures].[Units Prior YTD],[Measures].[Units Prior YTD Var %],
                        [Measures].[9L Eq Cases YTD],[Measures].[9L Eq Cases Prior YTD],[Measures].[9L Eq Cases Prior YTD Var %],
                            -----------------------------------
                        [Measures].[On Prem 13 Per TY] ,
                        [Measures].[On Prem 13 Per LY],
                        [Measures].[On Prem 13 Var %],
                        [Measures].[Rol Year Per TY$],
                        [Measures].[Rol Year $ Var %],                                             
                        [Measures].[Agent 9L Eq Cases MAT],[Measures].[Agent 9L Eq Cases MAT Pct], [Measures].[Agent Sales Amount MAT],[Measures].[Agent Sales Amount MAT Pct],[Measures].[Agent Units MAT],[Measures].[Agent Units MAT Pct],
                        [Measures].[Lic 9L Eq Cases MAT],[Measures].[Lic Sales Amount MAT],[Measures].[Lic Units MAT],
                                   [Measures].[Lic 9L Eq Cases MAT Pct],
                                   [Measures].[Lic Sales Amount MAT Pct],
                                   [Measures].[Lic Units MAT Pct]
                        } on columns//param
                        ,non empty(  [Product].[Master Product].allmembers  ) --on rows
                        having  [Measures].[Average Sale Price] >" + pricefrom + @" and [Measures].[Average Sale Price] <" + priceto + @"
                        on rows
                        from [LCBO Week]
                        WHERE 
                        (
                        @Period,
                        {@Category},
                        {@Varietal},
                        {@UnitSize}
                        )
            ";
        }

        public static string SalesSummaryMyCategoryCountry(decimal pricefrom, decimal priceto)
        {
            return @"
                       WITH
                        MEMBER [Measures].[Nb_General] as 0
                        MEMBER [Measures].[Nb_Vintages] as 0
                        MEMBER [Measures].[Nb_VintageEssential] as 0
                        //MEMBER [Measures].[9L Eq Cases CP TY] AS [Measures].[9L Eq Cases], FORMAT_STRING = '#,##0;(#,##0),0'
						//MEMBER [Measures].[9L Eq Cases CP LY] AS (ParallelPeriod([Date].[Promotional Period].CurrentMember.Level, 13,[Date].[Promotional Period].CurrentMember),[Measures].[9L Eq Cases]), FORMAT_STRING = '#,##0;(#,##0),0'
						MEMBER [Measures].[9L Eq Cases CP LY Var %] AS IIF(Round([9L Eq Cases CP LY],0) = 0, null, (Round([9L Eq Cases CP TY],0) - Round([Measures].[9L Eq Cases CP LY],0)) / Round([Measures].[9L Eq Cases CP LY],0)),format_string='Percent',visible = 1

						//MEMBER [Measures].[9L Eq Cases MAT] AS SUM({[Date].[Promotional Period].currentmember.lag(12):[Date].[Promotional Period].currentmember},[Measures].[9L Eq Cases]), FORMAT_STRING = '#,##0;(#,##0),0'
						//MEMBER [Measures].[9L Eq Cases Prior MAT] AS SUM({[Date].[Promotional Period].currentmember.lag(25):[Date].[Promotional Period].currentmember.lag(13)}, [Measures].[9L Eq Cases]), FORMAT_STRING = '#,##0;(#,##0),0'
						//MEMBER [Measures].[9L Eq Cases Prior MAT Var %] AS IIF(round([Measures].[9L Eq Cases Prior MAT],0) = 0, null, (round([Measures].[9L Eq Cases MAT],0) - round([Measures].[9L Eq Cases Prior MAT],0)) / round([Measures].[9L Eq Cases Prior MAT],0)),format_string='Percent',visible = 1

						//MEMBER [Measures].[9L Eq Cases YTD] AS SUM(PeriodsToDate([Date].[Promotional].[Year],[Date].[Promotional].currentmember),[Measures].[9L Eq Cases]), FORMAT_STRING = '#,##0;(#,##0),0'
						//MEMBER [Measures].[9L Eq Cases Prior YTD] AS SUM(PeriodstoDate([Date].[Promotional].[Year], ParallelPeriod([Date].[Promotional].[Year], 1,[Date].[Promotional].currentmember)),[Measures].[9L Eq Cases]), FORMAT_STRING = '#,##0;(#,##0),0'
						MEMBER [Measures].[9L Eq Cases Prior YTD Var %] AS IIF(Round([Measures].[9L Eq Cases Prior YTD],0) = 0, null, (Round([Measures].[9L Eq Cases YTD],0) - Round([Measures].[9L Eq Cases Prior YTD],0)) / Round([Measures].[9L Eq Cases Prior YTD],0)),format_string='Percent',visible = 1

						MEMBER [Measures].[On Prem 13 Per TY] AS SUM({[Date].[Promotional Period].currentmember.lag(12):[Date].[Promotional Period].currentmember},[Measures].[Units])
						MEMBER [Measures].[On Prem 13 Per LY] AS SUM({[Date].[Promotional Period].currentmember.lag(25):[Date].[Promotional Period].currentmember.lag(13)}, [Measures].[Units])
						MEMBER [Measures].[On Prem 13 Var %] AS IIF([Measures].[On Prem 13 Per TY] = 0, null, ([Measures].[On Prem 13 Per TY] - [Measures].[On Prem 13 Per LY]) / [Measures].[On Prem 13 Per TY]),format_string='Percent',visible = 1

						MEMBER [Measures].[Rol Year Per TY$] AS SUM({[Date].[Promotional Period].currentmember.lag(12):[Date].[Promotional Period].currentmember}, [Measures].[Sales Amount])
						MEMBER [Measures].[Rol Year $ Var %] AS IIF([Measures].[Rol Year Per TY$] = 0, null, ([Measures].[Rol Year Per TY$]-SUM({[Date].[Promotional Period].currentmember.lag(25):[Date].[Promotional Period].currentmember.lag(13)},[Measures].[Sales Amount])) / [Measures].[Rol Year Per TY$]), format_string='Percent',visible = 1

                        MEMBER [Measures].[Agent 9L Eq Cases MAT] as ([Measures].[9L Eq Cases MAT], [Account].[Channel].&[OFF TRADE]  )
                        MEMBER [Measures].[Lic 9L Eq Cases MAT] as ([Measures].[9L Eq Cases MAT], [Account].[Channel].&[ON TRADE]  )  
                        MEMBER [Measures].[Agent 9L Eq Cases MAT Pct] as ([Measures].[9L Eq Cases Prior MAT Var %] , [Account].[Channel].&[OFF TRADE]  )
                        MEMBER [Measures].[Lic 9L Eq Cases MAT Pct] as ([Measures].[9L Eq Cases Prior MAT Var %], [Account].[Channel].&[ON TRADE]  ) 
                        MEMBER [Measures].[Agent Sales Amount MAT] as ([Measures].[Sales Amount MAT] , [Account].[Channel].&[OFF TRADE]  )
                        MEMBER [Measures].[Lic Sales Amount MAT] as ( [Measures].[Sales Amount MAT], [Account].[Channel].&[ON TRADE]  )
                        MEMBER [Measures].[Agent Sales Amount MAT Pct] as ( [Measures].[Sales Amount Prior MAT Var %] , [Account].[Channel].&[OFF TRADE]  )
                        MEMBER [Measures].[Lic Sales Amount MAT Pct] as (  [Measures].[Sales Amount Prior MAT Var %], [Account].[Channel].&[ON TRADE]  )
                        MEMBER [Measures].[Agent Units MAT] as ( [Measures].[Units MAT] , [Account].[Channel].&[OFF TRADE]  )
                        MEMBER [Measures].[Lic Units MAT] as ( [Measures].[Units MAT] , [Account].[Channel].&[ON TRADE]  )    
                        MEMBER [Measures].[Agent Units MAT Pct] as ( [Measures].[Units Prior MAT Var %]  , [Account].[Channel].&[OFF TRADE]  )
                        MEMBER [Measures].[Lic Units MAT Pct] as (  [Measures].[Units Prior MAT Var %] , [Account].[Channel].&[ON TRADE]  )         

                        SELECT 
                        {
                            -----TY&LY----------------------
                        [Measures].[9L Eq Cases CP TY],[Measures].[9L Eq Cases CP LY],[Measures].[9L Eq Cases CP LY Var %],
                        [Measures].[Sales Amount CP TY],[Measures].[Sales Amount CP LY],[Measures].[Sales Amount CP LY Var %],
                        [Measures].[Units CP TY] ,[Measures].[Units CP LY],[Measures].[Units CP LY Var %],
                            -----6TY&LY----------------------
                        [Measures].[9L Eq Cases P6P],[Measures].[9L Eq Cases Prior P6P],[Measures].[9L Eq Cases Prior P6P Var %],
                        [Measures].[Sales Amount P6P],[Measures].[Sales Amount Prior P6P],[Measures].[Sales Amount Prior P6P Var %],
                        [Measures].[Units P6P],[Measures].[Units Prior P6P],[Measures].[Units Prior P6P Var %],
                            ----------1year-----------------
                        //[Measures].[9L Eq Cases MAT] ,[Measures].[9L Eq Cases Prior MAT],[Measures].[9L Eq Cases Prior MAT Var %],
                        //[Measures].[Sales Amount MAT],[Measures].[Sales Amount Prior MAT],[Measures].[Sales Amount Prior MAT Var %],
                        //[Measures].[Units MAT],[Measures].[Units Prior MAT],[Measures].[Units Prior MAT Var %],
                            ----------ytd----------------------
                        [Measures].[Sales Amount YTD],[Measures].[Sales Amount Prior YTD],[Measures].[Sales Amount Prior YTD Var %],
                        [Measures].[Units YTD],[Measures].[Units Prior YTD],[Measures].[Units Prior YTD Var %],
                        [Measures].[9L Eq Cases YTD],[Measures].[9L Eq Cases Prior YTD],[Measures].[9L Eq Cases Prior YTD Var %],
                            -----------------------------------
                        [Measures].[On Prem 13 Per TY] ,
                        [Measures].[On Prem 13 Per LY],
                        [Measures].[On Prem 13 Var %],
                        [Measures].[Rol Year Per TY$],
                        [Measures].[Rol Year $ Var %],                                             
                        [Measures].[Agent 9L Eq Cases MAT],[Measures].[Agent 9L Eq Cases MAT Pct], [Measures].[Agent Sales Amount MAT],[Measures].[Agent Sales Amount MAT Pct],[Measures].[Agent Units MAT],[Measures].[Agent Units MAT Pct],
                        [Measures].[Lic 9L Eq Cases MAT],[Measures].[Lic Sales Amount MAT],[Measures].[Lic Units MAT],
                                   [Measures].[Lic 9L Eq Cases MAT Pct],
                                   [Measures].[Lic Sales Amount MAT Pct],
                                   [Measures].[Lic Units MAT Pct]
                        } on columns//param
                        ,non empty( [Product].[Master Country].allmembers ) --on rows
                        having  [Measures].[Average Sale Price] >" + pricefrom + @" and [Measures].[Average Sale Price] <" + priceto + @"
                        on rows
                        from [LCBO Week]
                        WHERE 
                        (
                        @Period,
			            {@MyCategoryName},
                        {@Category},
                        {@UnitSize}
                        )

            ";
        }

        public static string SalesSummaryMyCategoryCountryProduct(decimal pricefrom, decimal priceto)
        {
            return @"
                       WITH
                        MEMBER [Measures].[Nb_General] as 0
                        MEMBER [Measures].[Nb_Vintages] as 0
                        MEMBER [Measures].[Nb_VintageEssential] as 0
                        //MEMBER [Measures].[9L Eq Cases CP LY] AS (ParallelPeriod([Date].[Promotional Period].CurrentMember.Level, 13,[Date].[Promotional Period].CurrentMember),[Measures].[9L Eq Cases]), FORMAT_STRING = '#,##0;(#,##0),0'
	                    MEMBER [Measures].[9L Eq Cases CP LY Var %] AS IIF(Round([9L Eq Cases CP LY],0) = 0, null, (Round([9L Eq Cases CP TY],0) - Round([Measures].[9L Eq Cases CP LY],0)) / Round([Measures].[9L Eq Cases CP LY],0)),format_string='Percent',visible = 1

	                    //MEMBER [Measures].[9L Eq Cases MAT] AS SUM({[Date].[Promotional Period].currentmember.lag(12):[Date].[Promotional Period].currentmember},[Measures].[9L Eq Cases]), FORMAT_STRING = '#,##0;(#,##0),0'
                        //MEMBER [Measures].[9L Eq Cases Prior MAT] AS SUM({[Date].[Promotional Period].currentmember.lag(25):[Date].[Promotional Period].currentmember.lag(13)}, [Measures].[9L Eq Cases]), FORMAT_STRING = '#,##0;(#,##0),0'
                        //MEMBER [Measures].[9L Eq Cases Prior MAT Var %] AS IIF(round([Measures].[9L Eq Cases Prior MAT],0) = 0, null, (round([Measures].[9L Eq Cases MAT],0) - round([Measures].[9L Eq Cases Prior MAT],0)) / round([Measures].[9L Eq Cases Prior MAT],0)),format_string='Percent',visible = 1

	                    //MEMBER [Measures].[9L Eq Cases YTD] AS SUM(PeriodsToDate([Date].[Promotional].[Year],[Date].[Promotional].currentmember),[Measures].[9L Eq Cases]), FORMAT_STRING = '#,##0;(#,##0),0'
	                    //MEMBER [Measures].[9L Eq Cases Prior YTD] AS SUM(PeriodstoDate([Date].[Promotional].[Year], ParallelPeriod([Date].[Promotional].[Year], 1,[Date].[Promotional].currentmember)),[Measures].[9L Eq Cases]), FORMAT_STRING = '#,##0;(#,##0),0'
	                    MEMBER [Measures].[9L Eq Cases Prior YTD Var %] AS IIF(Round([Measures].[9L Eq Cases Prior YTD],0) = 0, null, (Round([Measures].[9L Eq Cases YTD],0) - Round([Measures].[9L Eq Cases Prior YTD],0)) / Round([Measures].[9L Eq Cases Prior YTD],0)),format_string='Percent',visible = 1

                        MEMBER [Measures].[On Prem 13 Per TY] AS SUM({[Date].[Promotional Period].currentmember.lag(12):[Date].[Promotional Period].currentmember},[Measures].[Units])
                        MEMBER [Measures].[On Prem 13 Per LY] AS SUM({[Date].[Promotional Period].currentmember.lag(25):[Date].[Promotional Period].currentmember.lag(13)}, [Measures].[Units])
                        MEMBER [Measures].[On Prem 13 Var %] AS IIF([Measures].[On Prem 13 Per TY] = 0, null, ([Measures].[On Prem 13 Per TY] - [Measures].[On Prem 13 Per LY]) / [Measures].[On Prem 13 Per TY]),format_string='Percent',visible = 1

                        MEMBER [Measures].[Rol Year Per TY$] AS SUM({[Date].[Promotional Period].currentmember.lag(12):[Date].[Promotional Period].currentmember}, [Measures].[Sales Amount])
                        MEMBER [Measures].[Rol Year $ Var %] AS IIF([Measures].[Rol Year Per TY$] = 0, null, ([Measures].[Rol Year Per TY$]-SUM({[Date].[Promotional Period].currentmember.lag(25):[Date].[Promotional Period].currentmember.lag(13)},[Measures].[Sales Amount])) / [Measures].[Rol Year Per TY$]), format_string='Percent',visible = 1                        


                        MEMBER [Measures].[Agent 9L Eq Cases MAT] as ([Measures].[9L Eq Cases MAT], [Account].[Channel].&[OFF TRADE]  )
                        MEMBER [Measures].[Lic 9L Eq Cases MAT] as ([Measures].[9L Eq Cases MAT], [Account].[Channel].&[ON TRADE]  )  
                        MEMBER [Measures].[Agent 9L Eq Cases MAT Pct] as ([Measures].[9L Eq Cases Prior MAT Var %] , [Account].[Channel].&[OFF TRADE]  )
                        MEMBER [Measures].[Lic 9L Eq Cases MAT Pct] as ([Measures].[9L Eq Cases Prior MAT Var %], [Account].[Channel].&[ON TRADE]  ) 
                        MEMBER [Measures].[Agent Sales Amount MAT] as ([Measures].[Sales Amount MAT] , [Account].[Channel].&[OFF TRADE]  )
                        MEMBER [Measures].[Lic Sales Amount MAT] as ( [Measures].[Sales Amount MAT], [Account].[Channel].&[ON TRADE]  )
                        MEMBER [Measures].[Agent Sales Amount MAT Pct] as ( [Measures].[Sales Amount Prior MAT Var %] , [Account].[Channel].&[OFF TRADE]  )
                        MEMBER [Measures].[Lic Sales Amount MAT Pct] as (  [Measures].[Sales Amount Prior MAT Var %], [Account].[Channel].&[ON TRADE]  )
                        MEMBER [Measures].[Agent Units MAT] as ( [Measures].[Units MAT] , [Account].[Channel].&[OFF TRADE]  )
                        MEMBER [Measures].[Lic Units MAT] as ( [Measures].[Units MAT] , [Account].[Channel].&[ON TRADE]  )    
                        MEMBER [Measures].[Agent Units MAT Pct] as ( [Measures].[Units Prior MAT Var %]  , [Account].[Channel].&[OFF TRADE]  )
                        MEMBER [Measures].[Lic Units MAT Pct] as (  [Measures].[Units Prior MAT Var %] , [Account].[Channel].&[ON TRADE]  )         

                        SELECT 
                        {
                            -----TY&LY----------------------
                        [Measures].[9L Eq Cases CP TY],[Measures].[9L Eq Cases CP LY],[Measures].[9L Eq Cases CP LY Var %],
                        [Measures].[Sales Amount CP TY],[Measures].[Sales Amount CP LY],[Measures].[Sales Amount CP LY Var %],
                        [Measures].[Units CP TY] ,[Measures].[Units CP LY],[Measures].[Units CP LY Var %],
                            -----6TY&LY----------------------
                        [Measures].[9L Eq Cases P6P],[Measures].[9L Eq Cases Prior P6P],[Measures].[9L Eq Cases Prior P6P Var %],
                        [Measures].[Sales Amount P6P],[Measures].[Sales Amount Prior P6P],[Measures].[Sales Amount Prior P6P Var %],
                        [Measures].[Units P6P],[Measures].[Units Prior P6P],[Measures].[Units Prior P6P Var %],
                            ----------1year-----------------
                        //[Measures].[9L Eq Cases MAT] ,[Measures].[9L Eq Cases Prior MAT],[Measures].[9L Eq Cases Prior MAT Var %],
                        //[Measures].[Sales Amount MAT],[Measures].[Sales Amount Prior MAT],[Measures].[Sales Amount Prior MAT Var %],
                        //[Measures].[Units MAT],[Measures].[Units Prior MAT],[Measures].[Units Prior MAT Var %],
                            ----------ytd----------------------
                        [Measures].[Sales Amount YTD],[Measures].[Sales Amount Prior YTD],[Measures].[Sales Amount Prior YTD Var %],
                        [Measures].[Units YTD],[Measures].[Units Prior YTD],[Measures].[Units Prior YTD Var %],
                        [Measures].[9L Eq Cases YTD],[Measures].[9L Eq Cases Prior YTD],[Measures].[9L Eq Cases Prior YTD Var %],
                            -----------------------------------
                        [Measures].[On Prem 13 Per TY] ,
                        [Measures].[On Prem 13 Per LY],
                        [Measures].[On Prem 13 Var %],
                        [Measures].[Rol Year Per TY$],
                        [Measures].[Rol Year $ Var %],                                             
                        [Measures].[Agent 9L Eq Cases MAT],[Measures].[Agent 9L Eq Cases MAT Pct], [Measures].[Agent Sales Amount MAT],[Measures].[Agent Sales Amount MAT Pct],[Measures].[Agent Units MAT],[Measures].[Agent Units MAT Pct],
                        [Measures].[Lic 9L Eq Cases MAT],[Measures].[Lic Sales Amount MAT],[Measures].[Lic Units MAT],
                                   [Measures].[Lic 9L Eq Cases MAT Pct],
                                   [Measures].[Lic Sales Amount MAT Pct],
                                   [Measures].[Lic Units MAT Pct]
                        } on columns//param
                        ,non empty( [Product].[Master Product].allmembers)  --on rows
                        having  [Measures].[Average Sale Price] >" + pricefrom + @" and [Measures].[Average Sale Price] <" + priceto + @"
                        on rows
                        from [LCBO Week]
                        WHERE 
                        (
                        @Period,
			            {@MyCategoryName},
			            {@CountryName},
                        {@Category},
                        {@UnitSize}
                        )

            ";
        }


        public static string SalesSummaryPriceBandCategory(decimal pricefrom, decimal priceto)
        {
            return @"
                       WITH
                MEMBER [Measures].[Nb_General] as 0
                MEMBER [Measures].[Nb_Vintages] as 0
                MEMBER [Measures].[Nb_VintageEssential] as 0
                //MEMBER [Measures].[9L Eq Cases CP TY] AS [Measures].[9L Eq Cases], FORMAT_STRING = '#,##0;(#,##0),0'
				//MEMBER [Measures].[9L Eq Cases CP LY] AS (ParallelPeriod([Date].[Promotional Period].CurrentMember.Level, 13,[Date].[Promotional Period].CurrentMember),[Measures].[9L Eq Cases]), FORMAT_STRING = '#,##0;(#,##0),0'
				MEMBER [Measures].[9L Eq Cases CP LY Var %] AS IIF(Round([9L Eq Cases CP LY],0) = 0, null, (Round([9L Eq Cases CP TY],0) - Round([Measures].[9L Eq Cases CP LY],0)) / Round([Measures].[9L Eq Cases CP LY],0)),format_string='Percent',visible = 1

				//MEMBER [Measures].[9L Eq Cases MAT] AS SUM({[Date].[Promotional Period].currentmember.lag(12):[Date].[Promotional Period].currentmember},[Measures].[9L Eq Cases]), FORMAT_STRING = '#,##0;(#,##0),0'
				//MEMBER [Measures].[9L Eq Cases Prior MAT] AS SUM({[Date].[Promotional Period].currentmember.lag(25):[Date].[Promotional Period].currentmember.lag(13)}, [Measures].[9L Eq Cases]), FORMAT_STRING = '#,##0;(#,##0),0'
				//MEMBER [Measures].[9L Eq Cases Prior MAT Var %] AS IIF(round([Measures].[9L Eq Cases Prior MAT],0) = 0, null, (round([Measures].[9L Eq Cases MAT],0) - round([Measures].[9L Eq Cases Prior MAT],0)) / round([Measures].[9L Eq Cases Prior MAT],0)),format_string='Percent',visible = 1

				//MEMBER [Measures].[9L Eq Cases YTD] AS SUM(PeriodsToDate([Date].[Promotional].[Year],[Date].[Promotional].currentmember),[Measures].[9L Eq Cases]), FORMAT_STRING = '#,##0;(#,##0),0'
				//MEMBER [Measures].[9L Eq Cases Prior YTD] AS SUM(PeriodstoDate([Date].[Promotional].[Year], ParallelPeriod([Date].[Promotional].[Year], 1,[Date].[Promotional].currentmember)),[Measures].[9L Eq Cases]), FORMAT_STRING = '#,##0;(#,##0),0'
				MEMBER [Measures].[9L Eq Cases Prior YTD Var %] AS IIF(Round([Measures].[9L Eq Cases Prior YTD],0) = 0, null, (Round([Measures].[9L Eq Cases YTD],0) - Round([Measures].[9L Eq Cases Prior YTD],0)) / Round([Measures].[9L Eq Cases Prior YTD],0)),format_string='Percent',visible = 1

				MEMBER [Measures].[On Prem 13 Per TY] AS SUM({[Date].[Promotional Period].currentmember.lag(12):[Date].[Promotional Period].currentmember},[Measures].[Units])
				MEMBER [Measures].[On Prem 13 Per LY] AS SUM({[Date].[Promotional Period].currentmember.lag(25):[Date].[Promotional Period].currentmember.lag(13)}, [Measures].[Units])
				MEMBER [Measures].[On Prem 13 Var %] AS IIF([Measures].[On Prem 13 Per TY] = 0, null, ([Measures].[On Prem 13 Per TY] - [Measures].[On Prem 13 Per LY]) / [Measures].[On Prem 13 Per TY]),format_string='Percent',visible = 1

				MEMBER [Measures].[Rol Year Per TY$] AS SUM({[Date].[Promotional Period].currentmember.lag(12):[Date].[Promotional Period].currentmember}, [Measures].[Sales Amount])
				MEMBER [Measures].[Rol Year $ Var %] AS IIF([Measures].[Rol Year Per TY$] = 0, null, ([Measures].[Rol Year Per TY$]-SUM({[Date].[Promotional Period].currentmember.lag(25):[Date].[Promotional Period].currentmember.lag(13)},[Measures].[Sales Amount])) / [Measures].[Rol Year Per TY$]), format_string='Percent',visible = 1

                MEMBER [Measures].[Agent 9L Eq Cases MAT] as ([Measures].[9L Eq Cases MAT], [Account].[Channel].&[OFF TRADE]  )
                MEMBER [Measures].[Lic 9L Eq Cases MAT] as ([Measures].[9L Eq Cases MAT], [Account].[Channel].&[ON TRADE]  )  
                MEMBER [Measures].[Agent 9L Eq Cases MAT Pct] as ([Measures].[9L Eq Cases Prior MAT Var %] , [Account].[Channel].&[OFF TRADE]  )
                MEMBER [Measures].[Lic 9L Eq Cases MAT Pct] as ([Measures].[9L Eq Cases Prior MAT Var %], [Account].[Channel].&[ON TRADE]  ) 
                MEMBER [Measures].[Agent Sales Amount MAT] as ([Measures].[Sales Amount MAT] , [Account].[Channel].&[OFF TRADE]  )
                MEMBER [Measures].[Lic Sales Amount MAT] as ( [Measures].[Sales Amount MAT], [Account].[Channel].&[ON TRADE]  )
                MEMBER [Measures].[Agent Sales Amount MAT Pct] as ( [Measures].[Sales Amount Prior MAT Var %] , [Account].[Channel].&[OFF TRADE]  )
                MEMBER [Measures].[Lic Sales Amount MAT Pct] as (  [Measures].[Sales Amount Prior MAT Var %], [Account].[Channel].&[ON TRADE]  )
                MEMBER [Measures].[Agent Units MAT] as ( [Measures].[Units MAT] , [Account].[Channel].&[OFF TRADE]  )
                MEMBER [Measures].[Lic Units MAT] as ( [Measures].[Units MAT] , [Account].[Channel].&[ON TRADE]  )    
                MEMBER [Measures].[Agent Units MAT Pct] as ( [Measures].[Units Prior MAT Var %]  , [Account].[Channel].&[OFF TRADE]  )
                MEMBER [Measures].[Lic Units MAT Pct] as (  [Measures].[Units Prior MAT Var %] , [Account].[Channel].&[ON TRADE]  )         
                SELECT 
               non empty ( {          
                [Measures].[Nb_General],
                [Measures].[Nb_Vintages],
                [Measures].[Nb_VintageEssential] ,                    
                    -----TY&LY----------------------
                [Measures].[9L Eq Cases CP TY],[Measures].[9L Eq Cases CP LY],[Measures].[9L Eq Cases CP LY Var %],
                [Measures].[Sales Amount CP TY],[Measures].[Sales Amount CP LY],[Measures].[Sales Amount CP LY Var %],
                [Measures].[Units CP TY] ,[Measures].[Units CP LY],[Measures].[Units CP LY Var %],
                    -----6TY&LY----------------------
                [Measures].[9L Eq Cases P6P],[Measures].[9L Eq Cases Prior P6P],[Measures].[9L Eq Cases Prior P6P Var %],
                [Measures].[Sales Amount P6P],[Measures].[Sales Amount Prior P6P],[Measures].[Sales Amount Prior P6P Var %],
                [Measures].[Units P6P],[Measures].[Units Prior P6P],[Measures].[Units Prior P6P Var %],
                ----------1year-----------------
                //[Measures].[9L Eq Cases MAT] ,[Measures].[9L Eq Cases Prior MAT],[Measures].[9L Eq Cases Prior MAT Var %],
                //[Measures].[Sales Amount MAT],[Measures].[Sales Amount Prior MAT],[Measures].[Sales Amount Prior MAT Var %],
                //[Measures].[Units MAT],[Measures].[Units Prior MAT],[Measures].[Units Prior MAT Var %],
                 ----------ytd----------------------
                [Measures].[Sales Amount YTD],[Measures].[Sales Amount Prior YTD],[Measures].[Sales Amount Prior YTD Var %],
               [Measures].[Units YTD],[Measures].[Units Prior YTD],[Measures].[Units Prior YTD Var %],
                [Measures].[9L Eq Cases YTD],[Measures].[9L Eq Cases Prior YTD],[Measures].[9L Eq Cases Prior YTD Var %],
                -----------------------------------
                [Measures].[On Prem 13 Per TY] ,
                [Measures].[On Prem 13 Per LY],
                [Measures].[On Prem 13 Var %],
                [Measures].[Rol Year Per TY$],
                [Measures].[Rol Year $ Var %],                                                         
                [Measures].[Agent 9L Eq Cases MAT],[Measures].[Agent 9L Eq Cases MAT Pct], [Measures].[Agent Sales Amount MAT],[Measures].[Agent Sales Amount MAT Pct],[Measures].[Agent Units MAT],[Measures].[Agent Units MAT Pct],  
                [Measures].[Lic 9L Eq Cases MAT],[Measures].[Lic Sales Amount MAT],[Measures].[Lic Units MAT],[Measures].[Lic 9L Eq Cases MAT Pct],[Measures].[Lic Sales Amount MAT Pct],[Measures].[Lic Units MAT Pct]   
                     }) on columns,
               non empty(
                    [Product].[My Category].allmembers)
                having  [Measures].[Average Sale Price] >" + pricefrom + @" and [Measures].[Average Sale Price] <" + priceto + @"
                on rows
                FROM[LCBO Week]
                WHERE 
                (
                   @Period,
                  {@PriceBandName},
                  {@Category},
                  {@UnitSize}
                )
            ";
        }

        public static string SalesSummaryPriceBandMyCategoryProduct(decimal pricefrom, decimal priceto)
        {
            return @"
                       WITH
                MEMBER [Measures].[Nb_General] as 0
                MEMBER [Measures].[Nb_Vintages] as 0
                MEMBER [Measures].[Nb_VintageEssential] as 0
                //MEMBER [Measures].[9L Eq Cases CP TY] AS [Measures].[9L Eq Cases], FORMAT_STRING = '#,##0;(#,##0),0'
				//MEMBER [Measures].[9L Eq Cases CP LY] AS (ParallelPeriod([Date].[Promotional Period].CurrentMember.Level, 13,[Date].[Promotional Period].CurrentMember),[Measures].[9L Eq Cases]), FORMAT_STRING = '#,##0;(#,##0),0'
				MEMBER [Measures].[9L Eq Cases CP LY Var %] AS IIF(Round([9L Eq Cases CP LY],0) = 0, null, (Round([9L Eq Cases CP TY],0) - Round([Measures].[9L Eq Cases CP LY],0)) / Round([Measures].[9L Eq Cases CP LY],0)),format_string='Percent',visible = 1

				//MEMBER [Measures].[9L Eq Cases MAT] AS SUM({[Date].[Promotional Period].currentmember.lag(12):[Date].[Promotional Period].currentmember},[Measures].[9L Eq Cases]), FORMAT_STRING = '#,##0;(#,##0),0'
				//MEMBER [Measures].[9L Eq Cases Prior MAT] AS SUM({[Date].[Promotional Period].currentmember.lag(25):[Date].[Promotional Period].currentmember.lag(13)}, [Measures].[9L Eq Cases]), FORMAT_STRING = '#,##0;(#,##0),0'
				//MEMBER [Measures].[9L Eq Cases Prior MAT Var %] AS IIF(round([Measures].[9L Eq Cases Prior MAT],0) = 0, null, (round([Measures].[9L Eq Cases MAT],0) - round([Measures].[9L Eq Cases Prior MAT],0)) / round([Measures].[9L Eq Cases Prior MAT],0)),format_string='Percent',visible = 1

				//MEMBER [Measures].[9L Eq Cases YTD] AS SUM(PeriodsToDate([Date].[Promotional].[Year],[Date].[Promotional].currentmember),[Measures].[9L Eq Cases]), FORMAT_STRING = '#,##0;(#,##0),0'
				//MEMBER [Measures].[9L Eq Cases Prior YTD] AS SUM(PeriodstoDate([Date].[Promotional].[Year], ParallelPeriod([Date].[Promotional].[Year], 1,[Date].[Promotional].currentmember)),[Measures].[9L Eq Cases]), FORMAT_STRING = '#,##0;(#,##0),0'
				MEMBER [Measures].[9L Eq Cases Prior YTD Var %] AS IIF(Round([Measures].[9L Eq Cases Prior YTD],0) = 0, null, (Round([Measures].[9L Eq Cases YTD],0) - Round([Measures].[9L Eq Cases Prior YTD],0)) / Round([Measures].[9L Eq Cases Prior YTD],0)),format_string='Percent',visible = 1

				MEMBER [Measures].[On Prem 13 Per TY] AS SUM({[Date].[Promotional Period].currentmember.lag(12):[Date].[Promotional Period].currentmember},[Measures].[Units])
				MEMBER [Measures].[On Prem 13 Per LY] AS SUM({[Date].[Promotional Period].currentmember.lag(25):[Date].[Promotional Period].currentmember.lag(13)}, [Measures].[Units])
				MEMBER [Measures].[On Prem 13 Var %] AS IIF([Measures].[On Prem 13 Per TY] = 0, null, ([Measures].[On Prem 13 Per TY] - [Measures].[On Prem 13 Per LY]) / [Measures].[On Prem 13 Per TY]),format_string='Percent',visible = 1

				MEMBER [Measures].[Rol Year Per TY$] AS SUM({[Date].[Promotional Period].currentmember.lag(12):[Date].[Promotional Period].currentmember}, [Measures].[Sales Amount])
				MEMBER [Measures].[Rol Year $ Var %] AS IIF([Measures].[Rol Year Per TY$] = 0, null, ([Measures].[Rol Year Per TY$]-SUM({[Date].[Promotional Period].currentmember.lag(25):[Date].[Promotional Period].currentmember.lag(13)},[Measures].[Sales Amount])) / [Measures].[Rol Year Per TY$]), format_string='Percent',visible = 1

                MEMBER [Measures].[Agent 9L Eq Cases MAT] as ([Measures].[9L Eq Cases MAT], [Account].[Channel].&[OFF TRADE]  )
                MEMBER [Measures].[Lic 9L Eq Cases MAT] as ([Measures].[9L Eq Cases MAT], [Account].[Channel].&[ON TRADE]  )  
                MEMBER [Measures].[Agent 9L Eq Cases MAT Pct] as ([Measures].[9L Eq Cases Prior MAT Var %] , [Account].[Channel].&[OFF TRADE]  )
                MEMBER [Measures].[Lic 9L Eq Cases MAT Pct] as ([Measures].[9L Eq Cases Prior MAT Var %], [Account].[Channel].&[ON TRADE]  ) 
                MEMBER [Measures].[Agent Sales Amount MAT] as ([Measures].[Sales Amount MAT] , [Account].[Channel].&[OFF TRADE]  )
                MEMBER [Measures].[Lic Sales Amount MAT] as ( [Measures].[Sales Amount MAT], [Account].[Channel].&[ON TRADE]  )
                MEMBER [Measures].[Agent Sales Amount MAT Pct] as ( [Measures].[Sales Amount Prior MAT Var %] , [Account].[Channel].&[OFF TRADE]  )
                MEMBER [Measures].[Lic Sales Amount MAT Pct] as (  [Measures].[Sales Amount Prior MAT Var %], [Account].[Channel].&[ON TRADE]  )
                MEMBER [Measures].[Agent Units MAT] as ( [Measures].[Units MAT] , [Account].[Channel].&[OFF TRADE]  )
                MEMBER [Measures].[Lic Units MAT] as ( [Measures].[Units MAT] , [Account].[Channel].&[ON TRADE]  )    
                MEMBER [Measures].[Agent Units MAT Pct] as ( [Measures].[Units Prior MAT Var %]  , [Account].[Channel].&[OFF TRADE]  )
                MEMBER [Measures].[Lic Units MAT Pct] as (  [Measures].[Units Prior MAT Var %] , [Account].[Channel].&[ON TRADE]  )         
                SELECT 
               non empty ( {          
                [Measures].[Nb_General],
                [Measures].[Nb_Vintages],
                [Measures].[Nb_VintageEssential] ,                    
                    -----TY&LY----------------------
                [Measures].[9L Eq Cases CP TY],[Measures].[9L Eq Cases CP LY],[Measures].[9L Eq Cases CP LY Var %],
                [Measures].[Sales Amount CP TY],[Measures].[Sales Amount CP LY],[Measures].[Sales Amount CP LY Var %],
                [Measures].[Units CP TY] ,[Measures].[Units CP LY],[Measures].[Units CP LY Var %],
                    -----6TY&LY----------------------
                [Measures].[9L Eq Cases P6P],[Measures].[9L Eq Cases Prior P6P],[Measures].[9L Eq Cases Prior P6P Var %],
                [Measures].[Sales Amount P6P],[Measures].[Sales Amount Prior P6P],[Measures].[Sales Amount Prior P6P Var %],
                [Measures].[Units P6P],[Measures].[Units Prior P6P],[Measures].[Units Prior P6P Var %],
                ----------1year-----------------
                //[Measures].[9L Eq Cases MAT] ,[Measures].[9L Eq Cases Prior MAT],[Measures].[9L Eq Cases Prior MAT Var %],
                //[Measures].[Sales Amount MAT],[Measures].[Sales Amount Prior MAT],[Measures].[Sales Amount Prior MAT Var %],
                //[Measures].[Units MAT],[Measures].[Units Prior MAT],[Measures].[Units Prior MAT Var %],
                 ----------ytd----------------------
                [Measures].[Sales Amount YTD],[Measures].[Sales Amount Prior YTD],[Measures].[Sales Amount Prior YTD Var %],
                [Measures].[Units YTD],[Measures].[Units Prior YTD],[Measures].[Units Prior YTD Var %],
                [Measures].[9L Eq Cases YTD],[Measures].[9L Eq Cases Prior YTD],[Measures].[9L Eq Cases Prior YTD Var %],
                -----------------------------------
                [Measures].[On Prem 13 Per TY] ,
                [Measures].[On Prem 13 Per LY],
                [Measures].[On Prem 13 Var %],
                [Measures].[Rol Year Per TY$],
                [Measures].[Rol Year $ Var %],                                                         
                [Measures].[Agent 9L Eq Cases MAT],[Measures].[Agent 9L Eq Cases MAT Pct], [Measures].[Agent Sales Amount MAT],[Measures].[Agent Sales Amount MAT Pct],[Measures].[Agent Units MAT],[Measures].[Agent Units MAT Pct],  
                [Measures].[Lic 9L Eq Cases MAT],[Measures].[Lic Sales Amount MAT],[Measures].[Lic Units MAT],[Measures].[Lic 9L Eq Cases MAT Pct],[Measures].[Lic Sales Amount MAT Pct],[Measures].[Lic Units MAT Pct]   
                     }) on columns,
               non empty(
                    [Product].[Master Product].allmembers)
                having  [Measures].[Average Sale Price] >" + pricefrom + @" and [Measures].[Average Sale Price] <" + priceto + @"
                on rows
                FROM[LCBO Week]
                WHERE 
                (
                   @Period,
                  {@PriceBandName},
                  {@MyCategory},
                  {@Category},
                  {@UnitSize}
                )
            ";
        }

        public const string SalesCategoryStore = @"
                        WITH
                        SET    [OrderedSet] AS Order   (              
                            filter(
                            [Product].[Category].[Category].allmembers, iif (isempty([Measures].[Sales Amount Prior MAT Var %]),'a', [Measures].[Sales Amount Prior MAT Var %])<>'a' )
                            , iif (ISEMPTY([Measures].[Sales Amount MAT] ),0, [Measures].[Sales Amount MAT]), BDESC)   
                        MEMBER [Measures].[Rank] as Rank( [Product].[Category].CurrentMember , [OrderedSet]) 
                        MEMBER [Measures].[SalesAmount_PromoTurn_TY] AS iif (ISEMPTY([Measures].[Sales Amount CP TY]),0,[Measures].[Sales Amount CP TY])
                        MEMBER [Measures].[SalesAmount_PromoTurn_LY] AS iif (ISEMPTY([Measures].[Sales Amount CP LY]),0,[Measures].[Sales Amount CP LY])
                        MEMBER [Measures].[SalesAmount_PromoTurn_Pct] AS iif(ISEMPTY([Measures].[Sales Amount CP LY Var %]) or CStr([Measures].[Sales Amount CP LY Var %])='1.#INF', 0, [Measures].[Sales Amount CP LY Var %])
                        
                        MEMBER [Measures].[SalesAmount_6P_TY] AS iif (ISEMPTY([Measures].[Sales Amount P6P]),0, [Measures].[Sales Amount P6P])
                        MEMBER [Measures].[SalesAmount_6P_LY] AS iif (ISEMPTY([Measures].[Sales Amount Prior P6P]),0, [Measures].[Sales Amount Prior P6P]) 
                        MEMBER [Measures].[SalesAmount_6P_Pct] AS iif (ISEMPTY([Measures].[Sales Amount Prior P6P Var %]) or CStr([Measures].[Sales Amount Prior P6P Var %])='1.#INF',0, [Measures].[Sales Amount Prior P6P Var %])

                        MEMBER [Measures].[SalesAmount_13P_TY] AS iif (ISEMPTY([Measures].[Sales Amount MAT]),0, [Measures].[Sales Amount MAT]) 
                        MEMBER [Measures].[SalesAmount_13P_LY] AS iif (ISEMPTY([Measures].[Sales Amount Prior MAT]),0, [Measures].[Sales Amount Prior MAT]) 
                        MEMBER [Measures].[SalesAmount_13P_Pct] AS [Measures].[Sales Amount Prior MAT Var %]
                        MEMBER [Measures].[Promo List] as 0
                        MEMBER [Measures].[Inventory] as iif (ISEMPTY([Measures].[Inventory Count]),0, [Measures].[Inventory Count]) 
                        SELECT {
                                        [Measures].[Rank]
                                        , [Measures].[SalesAmount_PromoTurn_TY]
				                        , [Measures].[SalesAmount_PromoTurn_LY]
				                        , [Measures].[SalesAmount_PromoTurn_Pct]
				                        , [Measures].[SalesAmount_6P_TY]
				                        , [Measures].[SalesAmount_6P_LY]
				                        , [Measures].[SalesAmount_6P_Pct]
				                        , [Measures].[SalesAmount_13P_TY]
				                        , [Measures].[SalesAmount_13P_LY]
				                        , [Measures].[SalesAmount_13P_Pct]
				                       -- , [Measures].[Promo List]
				                        , [Measures].[Inventory]
                        }
                        ON COLUMNS 
                        ,NON EMPTY
                        filter(
                                Order ({[Product].[Category].[Category].allmembers}, [Measures].[Sales Amount MAT], DESC
                                    ) ,[Measures].[Rank]>0)
                        ON ROWS 
                        FROM  
                        [LCBO Week]
                        where
                        (
                        [Account].[Channel].&[OFF TRADE]
                        ,@Period
                        ,@AccountNumber
                        ,@ClientOnly
                        )
                        ";

        public const string SalesCategoryDetailStore = @"
                        WITH
                        MEMBER [Measures].[Listed/Delisted] as (IIF( [Measures].[Delist Count]  =1,'D','L'))
                        MEMBER [Measures].[SalesAmount_PromoTurn_TY] AS iif (ISEMPTY([Measures].[Sales Amount CP TY]),0,[Measures].[Sales Amount CP TY])
                        MEMBER [Measures].[SalesAmount_PromoTurn_LY] AS iif (ISEMPTY([Measures].[Sales Amount CP LY]),0,[Measures].[Sales Amount CP LY])
                        MEMBER [Measures].[SalesAmount_PromoTurn_Pct] AS iif(ISEMPTY([Measures].[Sales Amount CP LY Var %]), 0, [Measures].[Sales Amount CP LY Var %])
                        
                        MEMBER [Measures].[SalesAmount_6P_TY] AS iif (ISEMPTY([Measures].[Sales Amount P6P]),0, [Measures].[Sales Amount P6P])
                        MEMBER [Measures].[SalesAmount_6P_LY] AS iif (ISEMPTY([Measures].[Sales Amount Prior P6P]),0, [Measures].[Sales Amount Prior P6P]) 
                        MEMBER [Measures].[SalesAmount_6P_Pct] AS iif (ISEMPTY([Measures].[Sales Amount Prior P6P Var %]),0, [Measures].[Sales Amount Prior P6P Var %])

                        MEMBER [Measures].[SalesAmount_13P_TY] AS iif (ISEMPTY([Measures].[Sales Amount MAT]),0, [Measures].[Sales Amount MAT]) 
                        MEMBER [Measures].[SalesAmount_13P_LY] AS iif (ISEMPTY([Measures].[Sales Amount Prior MAT]),0, [Measures].[Sales Amount Prior MAT]) 
                        MEMBER [Measures].[SalesAmount_13P_Pct] AS [Measures].[Sales Amount Prior MAT Var %]                                                                       
                        MEMBER [Measures].[Promo List] as 0
                        MEMBER [Measures].[Inventory] as iif (ISEMPTY([Measures].[Inventory Count]),0, [Measures].[Inventory Count]) 
                        SELECT {
                                [Measures].[Listed/Delisted]
		                        ,[Measures].[SalesAmount_PromoTurn_TY]
		                        ,[Measures].[SalesAmount_PromoTurn_LY]
		                        ,[Measures].[SalesAmount_PromoTurn_Pct]
		                        ,[Measures].[SalesAmount_6P_TY]
		                        ,[Measures].[SalesAmount_6P_LY]
		                        ,[Measures].[SalesAmount_6P_Pct]
                                ,[Measures].[SalesAmount_13P_TY]  
                                ,[Measures].[SalesAmount_13P_LY]
                                ,[Measures].[SalesAmount_13P_Pct]
		                        ,[Measures].[Promo List]
		                        ,[Measures].[Inventory]
                        } ON COLUMNS 
                        , [Product].[CSPC].[CSPC].members
                        * [Product].[Product Name].[Product Name].members
                        * [Product].[Volume Per Unit ML].[Volume Per Unit ML].members
                        ON ROWS 
                        FROM  
                        [LCBO Week]
                        where  
                        (
                        [Account].[Channel].&[OFF TRADE]
                        ,@ClientOnly
                        ,@Period
                        ,@Category
                        ,@AccountNumber
                        )
                        ";

        public const string SalesCategoryLicensee = @"
                        WITH
                        MEMBER [Measures].[Calculated Sales] AS IIF(CStr([Measures].[Average Sale Price])='-1.#IND', 0, [Measures].[Average Sale Price])
                        MEMBER [Measures].[Price_13P_TY] AS  ( [Measures].[Calculated Sales], [Date].[Promotional].currentmember )
                        MEMBER [Measures].[Price_13P_LY] AS   ([Measures].[Calculated Sales],[Date].[Promotional].currentmember.prevmember)
                        MEMBER [Measures].[Avg_Retail_Price_TY] AS ( [Measures].[Calculated Sales],[Date].[Promotional].currentmember )
                        MEMBER [Measures].[Avg_Retail_Price_LY] AS ([Measures].[Calculated Sales],[Date].[Promotional].currentmember.prevmember)
                        MEMBER [Measures].[Avg_Retail_PricePct]  AS  
						
						iif( ISEMPTY([Measures].[Avg_Retail_Price_LY]) or [Measures].[Avg_Retail_Price_LY]=0, 0, ( ( [Measures].[Avg_Retail_Price_TY] )-([Measures].[Avg_Retail_Price_LY]))/([Measures].[Avg_Retail_Price_LY]))
                       
					   
                        MEMBER [Measures].[SalesAmount_13P_TY] AS iif (ISEMPTY([Measures].[Sales Amount MAT]),0, [Measures].[Sales Amount MAT]) 
                        MEMBER [Measures].[SalesAmount_13P_LY] AS iif (ISEMPTY([Measures].[Sales Amount Prior MAT]),0, [Measures].[Sales Amount Prior MAT]) 
                        MEMBER [Measures].[SalesAmount_13P_Pct] AS [Measures].[Sales Amount Prior MAT Var %]
                        MEMBER [Measures].[SalesAmount_TY] AS   iif (ISEMPTY([Measures].[Sales Amount CP TY]),0,[Measures].[Sales Amount CP TY]) 
                        MEMBER [Measures].[SalesAmount_LY] AS   iif (ISEMPTY([Measures].[Sales Amount CP LY]),0,[Measures].[Sales Amount CP LY])
                        SET    [OrderedSet] AS Order   (              
                                   filter(
                                   [Product].[Category].[Category].allmembers, iif (isempty([Measures].[Sales Amount Prior MAT Var %]),'a', [Measures].[Sales Amount Prior MAT Var %])<>'a' )
                                   , iif (ISEMPTY([Measures].[Sales Amount MAT] ),0, [Measures].[Sales Amount MAT]), BDESC)   
                        MEMBER [Measures].[Rank] as Rank( [Product].[Category].CurrentMember , [OrderedSet])
                        SELECT {
                                       [Measures].[Rank]
                                      ,[Measures].[Price_13P_TY]
                                      ,[Measures].[Price_13P_LY]
                                      ,[Measures].[SalesAmount_13P_TY]  
                                      ,[Measures].[SalesAmount_13P_LY]
                                      ,[Measures].[SalesAmount_13P_Pct]
                                      ,[Measures].[Avg_Retail_Price_TY]
                                      ,[Measures].[Avg_Retail_Price_LY]
                                      ,[Measures].[SalesAmount_TY]
                                      ,[Measures].[SalesAmount_LY]
                                      ,[Measures].[Avg_Retail_PricePct] 
                        }
                        ON COLUMNS 
                        ,NON EMPTY
                        filter(
                               Order ({[Product].[Category].[Category].allmembers}, [Measures].[Sales Amount MAT], DESC
                              ) ,[Measures].[Rank]>0)
                        ON ROWS 
                        FROM  
                        [LCBO Week]
                        where
                        (
                        [Account].[Channel].&[ON TRADE]
                        ,@Period
                        ,@AccountNumber
                        ,@ClientOnly
                        )

                        ";

        public const string SalesCategoryDetailLicensee = @"
                        WITH
                        MEMBER [Measures].[SalesAmount_13P_TY] AS iif (ISEMPTY([Measures].[Sales Amount MAT]),0, [Measures].[Sales Amount MAT]) 
                        MEMBER [Measures].[SalesAmount_13P_LY] AS iif (ISEMPTY([Measures].[Sales Amount Prior MAT]),0, [Measures].[Sales Amount Prior MAT]) 
                        MEMBER [Measures].[SalesAmount_13P_Pct] AS [Measures].[Sales Amount Prior MAT Var %]                    
                        SET    [OrderedSet]  
                        AS Order(
                        Filter (Order([Product].[Category].[Category].allmembers, [Measures].[Sales Amount MAT] ,BDESC) , 
                            iif (ISEMPTY( [Measures].[Sales Amount Prior MAT Var %]),'a' ,[Measures].[Sales Amount Prior MAT Var %]) <>'a'
                        )
                        ,iif(ISEMPTY([Measures].[Sales Amount Prior MAT Var %]),0,[Measures].[Sales Amount Prior MAT Var %]),BDESC)                                                        
                        MEMBER [Measures].[Rank] as Rank( [Product].[Category].CurrentMember , [OrderedSet])  
                        SET [ProductSet] 
                        AS ORDER(
                                filter( { [Product].[Category].[Category].MEMBERS  *  [Product].[Product Name].[Product Name].MEMBERS }
                                ,iif (ISEMPTY( [Measures].[Sales Amount Prior MAT Var %]),'a' ,[Measures].[Sales Amount Prior MAT Var %]) <>'a'
                                ), [Measures].[Sales Amount MAT],desc
                                )
                        MEMBER [Measures].[my_rank] AS RANK(([Product].[Category].currentmember,[Product].[Product Name].CURRENTMEMBER),[ProductSet])
                        MEMBER [Measures].[RankProduct] AS 
                            iif([ProductSet].ITEM([Measures].[my_rank]-1).ITEM(0).NAME <> [ProductSet].ITEM([Measures].[my_rank] -2).ITEM(0).NAME
                                , 1,([ProductSet].ITEM([Measures].[my_rank] -2), [Measures].[RankProduct])+1
                                )
                        MEMBER [Measures].[Listed/Delisted] as (IIF( [Measures].[Delist Count]  =1,'D','L'))
                        
                        MEMBER [Measures].[SalesAmount_6P_TY] AS iif (ISEMPTY([Measures].[Sales Amount P6P]),0, [Measures].[Sales Amount P6P])
                        MEMBER [Measures].[SalesAmount_6P_LY] AS iif (ISEMPTY([Measures].[Sales Amount Prior P6P]),0, [Measures].[Sales Amount Prior P6P]) 
                        MEMBER [Measures].[SalesAmount_6P_Pct] AS iif (ISEMPTY([Measures].[Sales Amount Prior P6P Var %]),0, [Measures].[Sales Amount Prior P6P Var %]) 
                        MEMBER [Measures].[Promo List] as 0
                        MEMBER [Measures].[Inventory] as iif (ISEMPTY([Measures].[Inventory Count]),0, [Measures].[Inventory Count]) 
                        SELECT {
                                [Measures].[Rank]
                                ,[Measures].[RankProduct]
		                        ,[Measures].[Listed/Delisted]
		                        ,[Measures].[SalesAmount_6P_TY]
		                        ,[Measures].[SalesAmount_6P_LY]
		                        ,[Measures].[SalesAmount_6P_Pct]
                                ,[Measures].[SalesAmount_13P_TY]  
                                ,[Measures].[SalesAmount_13P_LY]
                                ,[Measures].[SalesAmount_13P_Pct]
		                        ,[Measures].[Promo List]
		                        ,[Measures].[Inventory]
                        } ON COLUMNS 
                        , [ProductSet]
                        * [Product].[CSPC].[CSPC].members
                        * [Product].[Volume Per Unit ML].[Volume Per Unit ML].members
                        ON ROWS 
                        FROM  
                        [LCBO Week]
                        where  
                        (
                        [Account].[Channel].&[ON TRADE]
                        ,@ClientOnly
                        ,@Period
                        ,@AccountNumber
                        )
                        ";

        public const string SalesTeamSummaryAll = @"
                        WITH MEMBER [Measures].[Terr No] AS [Sales Rep].[Sales Rep].member_key
                        SELECT   {
                           [Measures].[Terr No]
                          ,[Measures].[9L Eq Cases MAT]
                          ,[Measures].[9L Eq Cases Prior MAT]
                          ,[Measures].[9L Eq Cases Prior MAT Var %]
                          ,[Measures].[Sales Amount MAT]
                          ,[Measures].[Sales Amount Prior MAT]
                          ,[Measures].[Sales Amount Prior MAT Var %]
                          ,[Measures].[Units MAT]
                          ,[Measures].[Units Prior MAT]
                          ,[Measures].[Units Prior MAT Var %]       
                        }  ON COLUMNS 
                        , NON EMPTY Filter([Sales Rep].[Sales Rep].members, NOT ISEMPTY([Measures].[9L Eq Cases MAT]))
                        ON ROWS              
                        FROM  [LCBO Week]
                        where  
                        (
                       // [Account].[Channel].&[OFF TRADE] 
                        @Period
                        ,[Product].[Is Own].&[1] -- Pelham Only
                        //,[Product].[Is Own].&[0], [Product].[Is Market].&[1] -- All Products
                        )
        ";

        public const string SalesTeamSummaryStore = @"
                        WITH MEMBER [Measures].[Terr No] AS [Sales Rep].[Sales Rep].member_key
                        SELECT   {
                           [Measures].[Terr No]
                          ,[Measures].[9L Eq Cases MAT]
                          ,[Measures].[9L Eq Cases Prior MAT]
                          ,[Measures].[9L Eq Cases Prior MAT Var %]
                          ,[Measures].[Sales Amount MAT]
                          ,[Measures].[Sales Amount Prior MAT]
                          ,[Measures].[Sales Amount Prior MAT Var %]
                          ,[Measures].[Units MAT]
                          ,[Measures].[Units Prior MAT]
                          ,[Measures].[Units Prior MAT Var %]       
                        }  ON COLUMNS 
                        , NON EMPTY Filter([Sales Rep].[Sales Rep].members, NOT ISEMPTY([Measures].[9L Eq Cases MAT]))
                        ON ROWS              
                        FROM  [LCBO Week]
                        where  
                        (
                        [Account].[Channel].&[OFF TRADE] 
                        ,@Period
                        ,[Product].[Is Own].&[1] -- Pelham Only
                        //,[Product].[Is Own].&[0], [Product].[Is Market].&[1] -- All Products
                        )
        ";

        public const string SalesTeamSummaryLicensee = @"
                        WITH MEMBER [Measures].[Terr No] AS [Sales Rep].[Sales Rep].member_key
                        SELECT   {
                           [Measures].[Terr No]
                          ,[Measures].[9L Eq Cases MAT]
                          ,[Measures].[9L Eq Cases Prior MAT]
                          ,[Measures].[9L Eq Cases Prior MAT Var %]
                          ,[Measures].[Sales Amount MAT]
                          ,[Measures].[Sales Amount Prior MAT]
                          ,[Measures].[Sales Amount Prior MAT Var %]
                          ,[Measures].[Units MAT]
                          ,[Measures].[Units Prior MAT]
                          ,[Measures].[Units Prior MAT Var %]       
                        }  ON COLUMNS 
                        , NON EMPTY Filter([Sales Rep].[Sales Rep].members, NOT ISEMPTY([Measures].[9L Eq Cases MAT]))
                        ON ROWS              
                        FROM  [LCBO Week]
                        where  
                        (
                        [Account].[Channel].&[ON TRADE] 
                        ,@Period
                        ,[Product].[Is Own].&[1] -- Pelham Only
                        //,[Product].[Is Own].&[0], [Product].[Is Market].&[1] -- All Products
                        )
        ";

        public const string SalesSummaryStoreByTerritory = @"
                        WITH
                        SET [SalesAmountSet] AS Order (filter( [Account].[Account Number].[Account Number].members, iif (isempty([Measures].[Sales Amount Prior MAT Var %]),'a', [Measures].[Sales Amount Prior MAT Var %])<>'a' ),[Measures].[Sales Amount MAT],BDESC)
                        MEMBER [Measures].[Rank Sales Amount] as Rank( [Account].[Account Number].CurrentMember , [SalesAmountSet]) 

                        SET [UnitsSet] AS Order (filter( [Account].[Account Number].[Account Number].members, iif (isempty([Measures].[Sales Amount Prior MAT Var %]),'a', [Measures].[Sales Amount Prior MAT Var %])<>'a' ),[Measures].[Units MAT],BDESC)
                        MEMBER [Measures].[Rank Units] as Rank( [Account].[Account Number].CurrentMember , [UnitsSet])

                        SET [9LEqCasesSet] AS Order (filter( [Account].[Account Number].[Account Number].members, iif (isempty([Measures].[Sales Amount Prior MAT Var %]),'a', [Measures].[Sales Amount Prior MAT Var %])<>'a' ),[Measures].[Measures].[9L Eq Cases MAT],BDESC)
                        MEMBER [Measures].[Rank 9L Eq Cases MAT] as Rank( [Account].[Account Number].CurrentMember , [9LEqCasesSet]) 

                        SELECT
                        {
                        [Measures].[Rank Sales Amount] 
                        ,[Measures].[Rank Units]
                        ,[Measures].[Rank 9L Eq Cases MAT]
                        ,[Measures].[9L Eq Cases MAT]
                        ,[Measures].[9L Eq Cases Prior MAT]
                        ,[Measures].[9L Eq Cases Prior MAT Var %]
                        ,[Measures].[Sales Amount MAT]
                        ,[Measures].[Sales Amount Prior MAT]
                        ,[Measures].[Sales Amount Prior MAT Var %]
                        ,[Measures].[Units MAT]
                        ,[Measures].[Units Prior MAT]
                        ,[Measures].[Units Prior MAT Var %]       
                        ,[Measures].[9L Eq Cases P3P]
                        ,[Measures].[9L Eq Cases Prior P3P]
                        ,[Measures].[9L Eq Cases Prior P3P Var %]
                        ,[Measures].[Sales Amount P3P]
                        ,[Measures].[Sales Amount Prior P3P]
                        ,[Measures].[Sales Amount Prior P3P Var %]
                        ,[Measures].[Units P3P]
                        ,[Measures].[Units Prior P3P]
                        ,[Measures].[Units Prior P3P Var %]
                        }
                        ON COLUMNS 
                        , NON EMPTY 
                        ORDER(
                               FILTER(
                                      [Account].[Account Number].[Account Number].members
                                      *[Account].[Account Name].[Account Name].members
                                      *[Account].[City].[City].members
                                      *[Account].[Address].[Address].members
			                          --[Sales Rep].[Sales Rep].&[32]
                                      ,[Measures].[9L Eq Cases MAT]>0
                                      )
                            ,[Measures].[Rank Sales Amount], BASC
                        )
                        ON ROWS              
                        FROM
                        [LCBO Week]
                        WHERE
                        (
                        [Account].[Channel].&[OFF TRADE] 
                        , @Period
                        , [Product].[Is Own].&[1] -- Pelham Only
                        , @UserId-- parameter from prev. page
                        )

        ";
        public const string SalesSummaryAllByTerritory = @"
                        WITH
                        SET [SalesAmountSet] AS Order (filter( [Account].[Account Number].[Account Number].members, iif (isempty([Measures].[Sales Amount Prior MAT Var %]),'a', [Measures].[Sales Amount Prior MAT Var %])<>'a' ),[Measures].[Sales Amount MAT],BDESC)
                        MEMBER [Measures].[Rank Sales Amount] as Rank( [Account].[Account Number].CurrentMember , [SalesAmountSet]) 

                        SET [UnitsSet] AS Order (filter( [Account].[Account Number].[Account Number].members, iif (isempty([Measures].[Sales Amount Prior MAT Var %]),'a', [Measures].[Sales Amount Prior MAT Var %])<>'a' ),[Measures].[Units MAT],BDESC)
                        MEMBER [Measures].[Rank Units] as Rank( [Account].[Account Number].CurrentMember , [UnitsSet])

                        SET [9LEqCasesSet] AS Order (filter( [Account].[Account Number].[Account Number].members, iif (isempty([Measures].[Sales Amount Prior MAT Var %]),'a', [Measures].[Sales Amount Prior MAT Var %])<>'a' ),[Measures].[Measures].[9L Eq Cases MAT],BDESC)
                        MEMBER [Measures].[Rank 9L Eq Cases MAT] as Rank( [Account].[Account Number].CurrentMember , [9LEqCasesSet]) 

                        SELECT
                        {
                        [Measures].[Rank Sales Amount] 
                        ,[Measures].[Rank Units]
                        ,[Measures].[Rank 9L Eq Cases MAT]
                        ,[Measures].[9L Eq Cases MAT]
                        ,[Measures].[9L Eq Cases Prior MAT]
                        ,[Measures].[9L Eq Cases Prior MAT Var %]
                        ,[Measures].[Sales Amount MAT]
                        ,[Measures].[Sales Amount Prior MAT]
                        ,[Measures].[Sales Amount Prior MAT Var %]
                        ,[Measures].[Units MAT]
                        ,[Measures].[Units Prior MAT]
                        ,[Measures].[Units Prior MAT Var %]       
                        ,[Measures].[9L Eq Cases P3P]
                        ,[Measures].[9L Eq Cases Prior P3P]
                        ,[Measures].[9L Eq Cases Prior P3P Var %]
                        ,[Measures].[Sales Amount P3P]
                        ,[Measures].[Sales Amount Prior P3P]
                        ,[Measures].[Sales Amount Prior P3P Var %]
                        ,[Measures].[Units P3P]
                        ,[Measures].[Units Prior P3P]
                        ,[Measures].[Units Prior P3P Var %]
                        }
                        ON COLUMNS 
                        , NON EMPTY 
                        ORDER(
                               FILTER(
                                      [Account].[Account Number].[Account Number].members
                                      *[Account].[Account Name].[Account Name].members
                                      *[Account].[City].[City].members
                                      *[Account].[Address].[Address].members
			                          --[Sales Rep].[Sales Rep].&[32]
                                      ,[Measures].[9L Eq Cases MAT]>0
                                      )
                            ,[Measures].[Rank Sales Amount], BASC
                        )
                        ON ROWS              
                        FROM
                        [LCBO Week]
                        WHERE
                        (
                       -- [Account].[Channel].&[OFF TRADE] 
                         @Period
                        , [Product].[Is Own].&[1] -- Pelham Only
                        , @UserId-- parameter from prev. page
                        )

        ";

        public const string SalesSummaryAllByTerritoryTotal = @"
                        WITH
                        SET [SalesAmountSet] AS Order (filter( [Account].[Account Number].[Account Number].members, iif (isempty([Measures].[Sales Amount Prior MAT Var %]),'a', [Measures].[Sales Amount Prior MAT Var %])<>'a' ),[Measures].[Sales Amount MAT],BDESC)
                        MEMBER [Measures].[Rank Sales Amount] as Rank( [Account].[Account Number].CurrentMember , [SalesAmountSet]) 

                        SET [UnitsSet] AS Order (filter( [Account].[Account Number].[Account Number].members, iif (isempty([Measures].[Sales Amount Prior MAT Var %]),'a', [Measures].[Sales Amount Prior MAT Var %])<>'a' ),[Measures].[Units MAT],BDESC)
                        MEMBER [Measures].[Rank Units] as Rank( [Account].[Account Number].CurrentMember , [UnitsSet])

                        SET [9LEqCasesSet] AS Order (filter( [Account].[Account Number].[Account Number].members, iif (isempty([Measures].[Sales Amount Prior MAT Var %]),'a', [Measures].[Sales Amount Prior MAT Var %])<>'a' ),[Measures].[Measures].[9L Eq Cases MAT],BDESC)
                        MEMBER [Measures].[Rank 9L Eq Cases MAT] as Rank( [Account].[Account Number].CurrentMember , [9LEqCasesSet]) 

                        SELECT
                        {
                        [Measures].[Rank Sales Amount] 
                        ,[Measures].[Rank Units]
                        ,[Measures].[Rank 9L Eq Cases MAT]
                        ,[Measures].[9L Eq Cases MAT]
                        ,[Measures].[9L Eq Cases Prior MAT]
                        ,[Measures].[9L Eq Cases Prior MAT Var %]
                        ,[Measures].[Sales Amount MAT]
                        ,[Measures].[Sales Amount Prior MAT]
                        ,[Measures].[Sales Amount Prior MAT Var %]
                        ,[Measures].[Units MAT]
                        ,[Measures].[Units Prior MAT]
                        ,[Measures].[Units Prior MAT Var %]       
                        ,[Measures].[9L Eq Cases P3P]
                        ,[Measures].[9L Eq Cases Prior P3P]
                        ,[Measures].[9L Eq Cases Prior P3P Var %]
                        ,[Measures].[Sales Amount P3P]
                        ,[Measures].[Sales Amount Prior P3P]
                        ,[Measures].[Sales Amount Prior P3P Var %]
                        ,[Measures].[Units P3P]
                        ,[Measures].[Units Prior P3P]
                        ,[Measures].[Units Prior P3P Var %]
                        }
                        ON COLUMNS 
                        , NON EMPTY 
                        ORDER(
                               FILTER(
			                          @UserId
                                      ,[Measures].[9L Eq Cases MAT]>0
                                      )
                            ,[Measures].[Rank Sales Amount], BASC
                        )
                        ON ROWS              
                        FROM
                        [LCBO Week]
                        WHERE
                        (
                       -- [Account].[Channel].&[OFF TRADE] 
                         @Period
                        , [Product].[Is Own].&[1] -- Pelham Only
                        )

        ";

        public const string SalesSummaryStoreByTerritoryTotal = @"
                        WITH
                        SET [SalesAmountSet] AS Order (filter( [Account].[Account Number].[Account Number].members, iif (isempty([Measures].[Sales Amount Prior MAT Var %]),'a', [Measures].[Sales Amount Prior MAT Var %])<>'a' ),[Measures].[Sales Amount MAT],BDESC)
                        MEMBER [Measures].[Rank Sales Amount] as Rank( [Account].[Account Number].CurrentMember , [SalesAmountSet]) 

                        SET [UnitsSet] AS Order (filter( [Account].[Account Number].[Account Number].members, iif (isempty([Measures].[Sales Amount Prior MAT Var %]),'a', [Measures].[Sales Amount Prior MAT Var %])<>'a' ),[Measures].[Units MAT],BDESC)
                        MEMBER [Measures].[Rank Units] as Rank( [Account].[Account Number].CurrentMember , [UnitsSet])

                        SET [9LEqCasesSet] AS Order (filter( [Account].[Account Number].[Account Number].members, iif (isempty([Measures].[Sales Amount Prior MAT Var %]),'a', [Measures].[Sales Amount Prior MAT Var %])<>'a' ),[Measures].[Measures].[9L Eq Cases MAT],BDESC)
                        MEMBER [Measures].[Rank 9L Eq Cases MAT] as Rank( [Account].[Account Number].CurrentMember , [9LEqCasesSet]) 

                        SELECT
                        {
                        [Measures].[Rank Sales Amount] 
                        ,[Measures].[Rank Units]
                        ,[Measures].[Rank 9L Eq Cases MAT]
                        ,[Measures].[9L Eq Cases MAT]
                        ,[Measures].[9L Eq Cases Prior MAT]
                        ,[Measures].[9L Eq Cases Prior MAT Var %]
                        ,[Measures].[Sales Amount MAT]
                        ,[Measures].[Sales Amount Prior MAT]
                        ,[Measures].[Sales Amount Prior MAT Var %]
                        ,[Measures].[Units MAT]
                        ,[Measures].[Units Prior MAT]
                        ,[Measures].[Units Prior MAT Var %]       
                        ,[Measures].[9L Eq Cases P3P]
                        ,[Measures].[9L Eq Cases Prior P3P]
                        ,[Measures].[9L Eq Cases Prior P3P Var %]
                        ,[Measures].[Sales Amount P3P]
                        ,[Measures].[Sales Amount Prior P3P]
                        ,[Measures].[Sales Amount Prior P3P Var %]
                        ,[Measures].[Units P3P]
                        ,[Measures].[Units Prior P3P]
                        ,[Measures].[Units Prior P3P Var %]
                        }
                        ON COLUMNS 
                        , NON EMPTY 
                        ORDER(
                               FILTER(
			                          @UserId
                                      ,[Measures].[9L Eq Cases MAT]>0
                                      )
                            ,[Measures].[Rank Sales Amount], BASC
                        )
                        ON ROWS              
                        FROM
                        [LCBO Week]
                        WHERE
                        (
                        [Account].[Channel].&[OFF TRADE] 
                        , @Period
                        , [Product].[Is Own].&[1] -- Pelham Only
                        )

        ";

        public const string SalesSummaryLicenseeByTerritory = @"
                        WITH
                        SET [SalesAmountSet] AS Order (filter( [Account].[Account Number].[Account Number].members, iif (isempty([Measures].[Sales Amount Prior MAT Var %]),'a', [Measures].[Sales Amount Prior MAT Var %])<>'a' ),[Measures].[Sales Amount MAT],BDESC)
                        MEMBER [Measures].[Rank Sales Amount] as Rank( [Account].[Account Number].CurrentMember , [SalesAmountSet]) 

                        SET [UnitsSet] AS Order (filter( [Account].[Account Number].[Account Number].members, iif (isempty([Measures].[Sales Amount Prior MAT Var %]),'a', [Measures].[Sales Amount Prior MAT Var %])<>'a' ),[Measures].[Units MAT],BDESC)
                        MEMBER [Measures].[Rank Units] as Rank( [Account].[Account Number].CurrentMember , [UnitsSet])

                        SET [9LEqCasesSet] AS Order (filter( [Account].[Account Number].[Account Number].members, iif (isempty([Measures].[Sales Amount Prior MAT Var %]),'a', [Measures].[Sales Amount Prior MAT Var %])<>'a' ),[Measures].[Measures].[9L Eq Cases MAT],BDESC)
                        MEMBER [Measures].[Rank 9L Eq Cases MAT] as Rank( [Account].[Account Number].CurrentMember , [9LEqCasesSet]) 

                        MEMBER [Measures].[9L 13P Direct Sales] as ([Measures].[9L Eq Cases MAT], [Feed].[Feed].&[DIRECT])
                        MEMBER [Measures].[9L 13P Direct Sales Prior] as ([Measures].[9L Eq Cases Prior MAT], [Feed].[Feed].&[DIRECT])
                        MEMBER [Measures].[9L 13P Direct Sales Prior Var] as ([Measures].[9L Eq Cases Prior MAT Var %], [Feed].[Feed].&[DIRECT])
                        MEMBER [Measures].[Sales 13P Direct Sales] as ([Measures].[Sales Amount MAT], [Feed].[Feed].&[DIRECT])
                        MEMBER [Measures].[Sales 13P Direct Sales Prior] as ([Measures].[Sales Amount Prior MAT], [Feed].[Feed].&[DIRECT])
                        MEMBER [Measures].[Sales 13P Direct Sales Prior Var] as ([Measures].[Sales Amount Prior MAT Var %], [Feed].[Feed].&[DIRECT])
                        MEMBER [Measures].[Units 13P Direct Sales] as ([Measures].[Units MAT], [Feed].[Feed].&[DIRECT])
                        MEMBER [Measures].[Units 13P Direct Sales Prior] as ([Measures].[Units Prior MAT], [Feed].[Feed].&[DIRECT])
                        MEMBER [Measures].[Units 13P Direct Sales Prior Var] as ([Measures].[Units Prior MAT Var %], [Feed].[Feed].&[DIRECT])

                        MEMBER [Measures].[9L 3P Direct Sales] as ([Measures].[9L Eq Cases P3P], [Feed].[Feed].&[DIRECT])
                        MEMBER [Measures].[9L 3P Direct Sales Prior] as ([Measures].[9L Eq Cases Prior P3P], [Feed].[Feed].&[DIRECT])
                        MEMBER [Measures].[9L 3P Direct Sales Prior Var] as ([Measures].[9L Eq Cases Prior P3P Var %], [Feed].[Feed].&[DIRECT])
                        MEMBER [Measures].[Sales 3P Direct Sales] as ([Measures].[Sales Amount P3P], [Feed].[Feed].&[DIRECT])
                        MEMBER [Measures].[Sales 3P Direct Sales Prior] as ([Measures].[Sales Amount Prior P3P], [Feed].[Feed].&[DIRECT])
                        MEMBER [Measures].[Sales 3P Direct Sales Prior Var] as ([Measures].[Sales Amount Prior P3P Var %], [Feed].[Feed].&[DIRECT])
                        MEMBER [Measures].[Units 3P Direct Sales] as ([Measures].[Units P3P], [Feed].[Feed].&[DIRECT])
                        MEMBER [Measures].[Units 3P Direct Sales Prior] as ([Measures].[Units Prior P3P], [Feed].[Feed].&[DIRECT])
                        MEMBER [Measures].[Units 3P Direct Sales Prior Var] as ([Measures].[Units Prior P3P Var %], [Feed].[Feed].&[DIRECT])

                        MEMBER [Measures].[9L 13P Licensee] as ([Measures].[9L Eq Cases MAT], [Feed].[Feed].&[LCBO])
                        MEMBER [Measures].[9L 13P Licensee Prior] as ([Measures].[9L Eq Cases Prior MAT], [Feed].[Feed].&[LCBO])
                        MEMBER [Measures].[9L 13P Licensee Prior Var] as ([Measures].[9L Eq Cases Prior MAT Var %], [Feed].[Feed].&[LCBO])
                        MEMBER [Measures].[Sales 13P Licensee] as ([Measures].[Sales Amount MAT], [Feed].[Feed].&[LCBO])
                        MEMBER [Measures].[Sales 13P Licensee Prior] as ([Measures].[Sales Amount Prior MAT], [Feed].[Feed].&[LCBO])
                        MEMBER [Measures].[Sales 13P Licensee Prior Var] as ([Measures].[Sales Amount Prior MAT Var %], [Feed].[Feed].&[LCBO])
                        MEMBER [Measures].[Units 13P Licensee] as ([Measures].[Units MAT], [Feed].[Feed].&[LCBO])
                        MEMBER [Measures].[Units 13P Licensee Prior] as ([Measures].[Units Prior MAT], [Feed].[Feed].&[LCBO])
                        MEMBER [Measures].[Units 13P Licensee Prior Var] as ([Measures].[Units Prior MAT Var %], [Feed].[Feed].&[LCBO])

                        MEMBER [Measures].[9L 3P Licensee] as ([Measures].[9L Eq Cases P3P], [Feed].[Feed].&[LCBO])
                        MEMBER [Measures].[9L 3P Licensee Prior] as ([Measures].[9L Eq Cases Prior P3P], [Feed].[Feed].&[LCBO])
                        MEMBER [Measures].[9L 3P Licensee Prior Var] as ([Measures].[9L Eq Cases Prior P3P Var %], [Feed].[Feed].&[LCBO])
                        MEMBER [Measures].[Sales 3P Licensee] as ([Measures].[Sales Amount P3P], [Feed].[Feed].&[LCBO])
                        MEMBER [Measures].[Sales 3P Licensee Prior] as ([Measures].[Sales Amount Prior P3P], [Feed].[Feed].&[LCBO])
                        MEMBER [Measures].[Sales 3P Licensee Prior Var] as ([Measures].[Sales Amount Prior P3P Var %], [Feed].[Feed].&[LCBO])
                        MEMBER [Measures].[Units 3P Licensee] as ([Measures].[Units P3P], [Feed].[Feed].&[LCBO])
                        MEMBER [Measures].[Units 3P Licensee Prior] as ([Measures].[Units Prior P3P], [Feed].[Feed].&[LCBO])
                        MEMBER [Measures].[Units 3P Licensee Prior Var] as ([Measures].[Units Prior P3P Var %], [Feed].[Feed].&[LCBO])

                        SELECT
                        {
                        [Measures].[Rank Sales Amount] 
                        ,[Measures].[Rank Units]
                        ,[Measures].[Rank 9L Eq Cases MAT]

                        ,[Measures].[9L Eq Cases MAT]
                        ,[Measures].[9L Eq Cases Prior MAT]
                        ,[Measures].[9L Eq Cases Prior MAT Var %]
                        ,[Measures].[Sales Amount MAT]
                        ,[Measures].[Sales Amount Prior MAT]
                        ,[Measures].[Sales Amount Prior MAT Var %]
                        ,[Measures].[Units MAT]
                        ,[Measures].[Units Prior MAT]
                        ,[Measures].[Units Prior MAT Var %]       
                        ,[Measures].[9L Eq Cases P3P]
                        ,[Measures].[9L Eq Cases Prior P3P]
                        ,[Measures].[9L Eq Cases Prior P3P Var %]
                        ,[Measures].[Sales Amount P3P]
                        ,[Measures].[Sales Amount Prior P3P]
                        ,[Measures].[Sales Amount Prior P3P Var %]
                        ,[Measures].[Units P3P]
                        ,[Measures].[Units Prior P3P]
                        ,[Measures].[Units Prior P3P Var %]

                        ,[Measures].[9L 13P Direct Sales]
                        ,[Measures].[9L 13P Direct Sales Prior]
                        ,[Measures].[9L 13P Direct Sales Prior Var]
                        ,[Measures].[Sales 13P Direct Sales]
                        ,[Measures].[Sales 13P Direct Sales Prior]
                        ,[Measures].[Sales 13P Direct Sales Prior Var]
                        ,[Measures].[Units 13P Direct Sales]
                        ,[Measures].[Units 13P Direct Sales Prior]
                        ,[Measures].[Units 13P Direct Sales Prior Var]

                        ,[Measures].[9L 3P Direct Sales]
                        ,[Measures].[9L 3P Direct Sales Prior]
                        ,[Measures].[9L 3P Direct Sales Prior Var]
                        ,[Measures].[Sales 3P Direct Sales]
                        ,[Measures].[Sales 3P Direct Sales Prior]
                        ,[Measures].[Sales 3P Direct Sales Prior Var]
                        ,[Measures].[Units 3P Direct Sales]
                        ,[Measures].[Units 3P Direct Sales Prior]
                        ,[Measures].[Units 3P Direct Sales Prior Var]

                        ,[Measures].[9L 13P Licensee]
                        ,[Measures].[9L 13P Licensee Prior]
                        ,[Measures].[9L 13P Licensee Prior Var]
                        ,[Measures].[Sales 13P Licensee]
                        ,[Measures].[Sales 13P Licensee Prior]
                        ,[Measures].[Sales 13P Licensee Prior Var]
                        ,[Measures].[Units 13P Licensee]
                        ,[Measures].[Units 13P Licensee Prior]
                        ,[Measures].[Units 13P Licensee Prior Var]

                        ,[Measures].[9L 3P Licensee]
                        ,[Measures].[9L 3P Licensee Prior]
                        ,[Measures].[9L 3P Licensee Prior Var]
                        ,[Measures].[Sales 3P Licensee]
                        ,[Measures].[Sales 3P Licensee Prior]
                        ,[Measures].[Sales 3P Licensee Prior Var]
                        ,[Measures].[Units 3P Licensee]
                        ,[Measures].[Units 3P Licensee Prior]
                        ,[Measures].[Units 3P Licensee Prior Var]
                        }
                        ON COLUMNS  
                        , NON EMPTY 
                        ORDER(
                               FILTER(
			                          [Account].[Account Number].[Account Number].members
                                      * [Account].[Account Name].[Account Name].members
                                      * [Account].[City].[City].members
                                      * [Account].[Address].[Address].MEMBERS
                                      ,[Measures].[9L Eq Cases MAT]>0
                                      )
                            ,[Measures].[Rank Sales Amount], BASC
                        )
                        ON ROWS              
                        FROM
                        [LCBO Week]
                        WHERE
                        (
                        [Account].[Channel].&[ON TRADE] 
                        , @Period
                        , [Product].[Is Own].&[1] -- Pelham Only
                        , @UserId
                        )

        ";

        public const string SalesSummaryLicenseeByTerritoryTotal = @"
                        WITH
                        SET [SalesAmountSet] AS Order (filter( [Account].[Account Number].[Account Number].members, iif (isempty([Measures].[Sales Amount Prior MAT Var %]),'a', [Measures].[Sales Amount Prior MAT Var %])<>'a' ),[Measures].[Sales Amount MAT],BDESC)
                        MEMBER [Measures].[Rank Sales Amount] as Rank( [Account].[Account Number].CurrentMember , [SalesAmountSet]) 

                        SET [UnitsSet] AS Order (filter( [Account].[Account Number].[Account Number].members, iif (isempty([Measures].[Sales Amount Prior MAT Var %]),'a', [Measures].[Sales Amount Prior MAT Var %])<>'a' ),[Measures].[Units MAT],BDESC)
                        MEMBER [Measures].[Rank Units] as Rank( [Account].[Account Number].CurrentMember , [UnitsSet])

                        SET [9LEqCasesSet] AS Order (filter( [Account].[Account Number].[Account Number].members, iif (isempty([Measures].[Sales Amount Prior MAT Var %]),'a', [Measures].[Sales Amount Prior MAT Var %])<>'a' ),[Measures].[Measures].[9L Eq Cases MAT],BDESC)
                        MEMBER [Measures].[Rank 9L Eq Cases MAT] as Rank( [Account].[Account Number].CurrentMember , [9LEqCasesSet]) 

                        MEMBER [Measures].[9L 13P Direct Sales] as ([Measures].[9L Eq Cases MAT], [Feed].[Feed].&[DIRECT])
                        MEMBER [Measures].[9L 13P Direct Sales Prior] as ([Measures].[9L Eq Cases Prior MAT], [Feed].[Feed].&[DIRECT])
                        MEMBER [Measures].[9L 13P Direct Sales Prior Var] as ([Measures].[9L Eq Cases Prior MAT Var %], [Feed].[Feed].&[DIRECT])
                        MEMBER [Measures].[Sales 13P Direct Sales] as ([Measures].[Sales Amount MAT], [Feed].[Feed].&[DIRECT])
                        MEMBER [Measures].[Sales 13P Direct Sales Prior] as ([Measures].[Sales Amount Prior MAT], [Feed].[Feed].&[DIRECT])
                        MEMBER [Measures].[Sales 13P Direct Sales Prior Var] as ([Measures].[Sales Amount Prior MAT Var %], [Feed].[Feed].&[DIRECT])
                        MEMBER [Measures].[Units 13P Direct Sales] as ([Measures].[Units MAT], [Feed].[Feed].&[DIRECT])
                        MEMBER [Measures].[Units 13P Direct Sales Prior] as ([Measures].[Units Prior MAT], [Feed].[Feed].&[DIRECT])
                        MEMBER [Measures].[Units 13P Direct Sales Prior Var] as ([Measures].[Units Prior MAT Var %], [Feed].[Feed].&[DIRECT])

                        MEMBER [Measures].[9L 3P Direct Sales] as ([Measures].[9L Eq Cases P3P], [Feed].[Feed].&[DIRECT])
                        MEMBER [Measures].[9L 3P Direct Sales Prior] as ([Measures].[9L Eq Cases Prior P3P], [Feed].[Feed].&[DIRECT])
                        MEMBER [Measures].[9L 3P Direct Sales Prior Var] as ([Measures].[9L Eq Cases Prior P3P Var %], [Feed].[Feed].&[DIRECT])
                        MEMBER [Measures].[Sales 3P Direct Sales] as ([Measures].[Sales Amount P3P], [Feed].[Feed].&[DIRECT])
                        MEMBER [Measures].[Sales 3P Direct Sales Prior] as ([Measures].[Sales Amount Prior P3P], [Feed].[Feed].&[DIRECT])
                        MEMBER [Measures].[Sales 3P Direct Sales Prior Var] as ([Measures].[Sales Amount Prior P3P Var %], [Feed].[Feed].&[DIRECT])
                        MEMBER [Measures].[Units 3P Direct Sales] as ([Measures].[Units P3P], [Feed].[Feed].&[DIRECT])
                        MEMBER [Measures].[Units 3P Direct Sales Prior] as ([Measures].[Units Prior P3P], [Feed].[Feed].&[DIRECT])
                        MEMBER [Measures].[Units 3P Direct Sales Prior Var] as ([Measures].[Units Prior P3P Var %], [Feed].[Feed].&[DIRECT])

                        MEMBER [Measures].[9L 13P Licensee] as ([Measures].[9L Eq Cases MAT], [Feed].[Feed].&[LCBO])
                        MEMBER [Measures].[9L 13P Licensee Prior] as ([Measures].[9L Eq Cases Prior MAT], [Feed].[Feed].&[LCBO])
                        MEMBER [Measures].[9L 13P Licensee Prior Var] as ([Measures].[9L Eq Cases Prior MAT Var %], [Feed].[Feed].&[LCBO])
                        MEMBER [Measures].[Sales 13P Licensee] as ([Measures].[Sales Amount MAT], [Feed].[Feed].&[LCBO])
                        MEMBER [Measures].[Sales 13P Licensee Prior] as ([Measures].[Sales Amount Prior MAT], [Feed].[Feed].&[LCBO])
                        MEMBER [Measures].[Sales 13P Licensee Prior Var] as ([Measures].[Sales Amount Prior MAT Var %], [Feed].[Feed].&[LCBO])
                        MEMBER [Measures].[Units 13P Licensee] as ([Measures].[Units MAT], [Feed].[Feed].&[LCBO])
                        MEMBER [Measures].[Units 13P Licensee Prior] as ([Measures].[Units Prior MAT], [Feed].[Feed].&[LCBO])
                        MEMBER [Measures].[Units 13P Licensee Prior Var] as ([Measures].[Units Prior MAT Var %], [Feed].[Feed].&[LCBO])

                        MEMBER [Measures].[9L 3P Licensee] as ([Measures].[9L Eq Cases P3P], [Feed].[Feed].&[LCBO])
                        MEMBER [Measures].[9L 3P Licensee Prior] as ([Measures].[9L Eq Cases Prior P3P], [Feed].[Feed].&[LCBO])
                        MEMBER [Measures].[9L 3P Licensee Prior Var] as ([Measures].[9L Eq Cases Prior P3P Var %], [Feed].[Feed].&[LCBO])
                        MEMBER [Measures].[Sales 3P Licensee] as ([Measures].[Sales Amount P3P], [Feed].[Feed].&[LCBO])
                        MEMBER [Measures].[Sales 3P Licensee Prior] as ([Measures].[Sales Amount Prior P3P], [Feed].[Feed].&[LCBO])
                        MEMBER [Measures].[Sales 3P Licensee Prior Var] as ([Measures].[Sales Amount Prior P3P Var %], [Feed].[Feed].&[LCBO])
                        MEMBER [Measures].[Units 3P Licensee] as ([Measures].[Units P3P], [Feed].[Feed].&[LCBO])
                        MEMBER [Measures].[Units 3P Licensee Prior] as ([Measures].[Units Prior P3P], [Feed].[Feed].&[LCBO])
                        MEMBER [Measures].[Units 3P Licensee Prior Var] as ([Measures].[Units Prior P3P Var %], [Feed].[Feed].&[LCBO])

                        SELECT
                        {
                        [Measures].[Rank Sales Amount] 
                        ,[Measures].[Rank Units]
                        ,[Measures].[Rank 9L Eq Cases MAT]

                        ,[Measures].[9L Eq Cases MAT]
                        ,[Measures].[9L Eq Cases Prior MAT]
                        ,[Measures].[9L Eq Cases Prior MAT Var %]
                        ,[Measures].[Sales Amount MAT]
                        ,[Measures].[Sales Amount Prior MAT]
                        ,[Measures].[Sales Amount Prior MAT Var %]
                        ,[Measures].[Units MAT]
                        ,[Measures].[Units Prior MAT]
                        ,[Measures].[Units Prior MAT Var %]       
                        ,[Measures].[9L Eq Cases P3P]
                        ,[Measures].[9L Eq Cases Prior P3P]
                        ,[Measures].[9L Eq Cases Prior P3P Var %]
                        ,[Measures].[Sales Amount P3P]
                        ,[Measures].[Sales Amount Prior P3P]
                        ,[Measures].[Sales Amount Prior P3P Var %]
                        ,[Measures].[Units P3P]
                        ,[Measures].[Units Prior P3P]
                        ,[Measures].[Units Prior P3P Var %]

                        ,[Measures].[9L 13P Direct Sales]
                        ,[Measures].[9L 13P Direct Sales Prior]
                        ,[Measures].[9L 13P Direct Sales Prior Var]
                        ,[Measures].[Sales 13P Direct Sales]
                        ,[Measures].[Sales 13P Direct Sales Prior]
                        ,[Measures].[Sales 13P Direct Sales Prior Var]
                        ,[Measures].[Units 13P Direct Sales]
                        ,[Measures].[Units 13P Direct Sales Prior]
                        ,[Measures].[Units 13P Direct Sales Prior Var]

                        ,[Measures].[9L 3P Direct Sales]
                        ,[Measures].[9L 3P Direct Sales Prior]
                        ,[Measures].[9L 3P Direct Sales Prior Var]
                        ,[Measures].[Sales 3P Direct Sales]
                        ,[Measures].[Sales 3P Direct Sales Prior]
                        ,[Measures].[Sales 3P Direct Sales Prior Var]
                        ,[Measures].[Units 3P Direct Sales]
                        ,[Measures].[Units 3P Direct Sales Prior]
                        ,[Measures].[Units 3P Direct Sales Prior Var]

                        ,[Measures].[9L 13P Licensee]
                        ,[Measures].[9L 13P Licensee Prior]
                        ,[Measures].[9L 13P Licensee Prior Var]
                        ,[Measures].[Sales 13P Licensee]
                        ,[Measures].[Sales 13P Licensee Prior]
                        ,[Measures].[Sales 13P Licensee Prior Var]
                        ,[Measures].[Units 13P Licensee]
                        ,[Measures].[Units 13P Licensee Prior]
                        ,[Measures].[Units 13P Licensee Prior Var]

                        ,[Measures].[9L 3P Licensee]
                        ,[Measures].[9L 3P Licensee Prior]
                        ,[Measures].[9L 3P Licensee Prior Var]
                        ,[Measures].[Sales 3P Licensee]
                        ,[Measures].[Sales 3P Licensee Prior]
                        ,[Measures].[Sales 3P Licensee Prior Var]
                        ,[Measures].[Units 3P Licensee]
                        ,[Measures].[Units 3P Licensee Prior]
                        ,[Measures].[Units 3P Licensee Prior Var]
                        }
                        ON COLUMNS  
                        , NON EMPTY 
                        ORDER(
                               FILTER(
			                          @UserId
                                      ,[Measures].[9L Eq Cases MAT]>0
                                      )
                            ,[Measures].[Rank Sales Amount], BASC
                        )
                        ON ROWS              
                        FROM
                        [LCBO Week]
                        WHERE
                        (
                        [Account].[Channel].&[ON TRADE] 
                        , @Period
                        , [Product].[Is Own].&[1] -- Pelham Only
                        )

        ";

        public const string SalesTeamStoreProduct = @"
                        WITH 
                        MEMBER [Measures].[Promo List] as '0' -- GENERATE( NonEmpty( { [Promotion].[Promotion Code].[Promotion Code].Members}, [Measures].[Sales Amount CP TY] ), [Promotion].[Promotion Code].currentmember.name, ', ' )
                        MEMBER [Measures].[Listed/Delisted] as (IIF( [Measures].[Delist Count]  =1,'D','L'))
                        MEMBER [Measures].[Inventory] as iif (ISEMPTY([Measures].[Inventory Count]),0, [Measures].[Inventory Count]) 

                        SELECT   {
                        [Measures].[Listed/Delisted]
                        ,[Measures].[9L Eq Cases CP TY]
                        ,[Measures].[9L Eq Cases CP LY]
                        ,[Measures].[9L Eq Cases CP LY Var %]
                        ,[Measures].[Sales Amount CP TY]
                        ,[Measures].[Sales Amount CP LY]
                        ,[Measures].[Sales Amount CP LY Var %]
                        ,[Measures].[Units CP TY]
                        ,[Measures].[Units CP LY]
                        ,[Measures].[Units CP LY Var %]
                        ,[Measures].[9L Eq Cases P6P]
                        ,[Measures].[9L Eq Cases Prior P6P]
                        ,[Measures].[9L Eq Cases Prior P6P Var %]
                        ,[Measures].[Sales Amount P6P]
                        ,[Measures].[Sales Amount Prior P6P]
                        ,[Measures].[Sales Amount Prior P6P Var %]
                        ,[Measures].[Units P6P]
                        ,[Measures].[Units Prior P6P]
                        ,[Measures].[Units Prior P6P Var %]
                        ,[Measures].[9L Eq Cases MAT]
                        ,[Measures].[9L Eq Cases Prior MAT]
                        ,[Measures].[9L Eq Cases Prior MAT Var %]
                        ,[Measures].[Sales Amount MAT]
                        ,[Measures].[Sales Amount Prior MAT]
                        ,[Measures].[Sales Amount Prior MAT Var %]
                        ,[Measures].[Units MAT]
                        ,[Measures].[Units Prior MAT]
                        ,[Measures].[Units Prior MAT Var %]  
                        ,[Measures].[Promo List]
                        ,[Measures].[Inventory]
                        }
                        ON COLUMNS
                        , NON EMPTY
                        FILTER(
                                [Product].[CSPC].[CSPC].members
                                * [Product].[Product Name].[Product Name].members
                                * [Product].[Volume Per Unit ML].[Volume Per Unit ML].members
                                * [Product].[Category].[Category].members
                                , IIF(ISEMPTY([Measures].[Sales Amount Prior MAT Var %]), 'a', [Measures].[Sales Amount Prior MAT Var %])
	                    <>'a')
                        ON ROWS
                        FROM [LCBO Week]
                        where
                        (
                        [Account].[Channel].&[OFF TRADE]
                        , @Period
                       // , [Product].[Is Own].&[1] -- Pelham Only
                        //, @UserId -- parameter from prev.page
                        , @AccountNumber -- parameter from prev.page
                        ,@ClientOnly
                        )

        ";

        public const string SalesTeamStoreProductTotal = @"
                        WITH 
                        MEMBER [Measures].[Promo List] as 0-- GENERATE( NonEmpty( { [Promotion].[Promotion Code].[Promotion Code].Members}, [Measures].[Sales Amount CP TY] ), [Promotion].[Promotion Code].currentmember.name, ', ' )
                        MEMBER [Measures].[Listed/Delisted] as (IIF( [Measures].[Delist Count]  =1,'D','L'))
                        MEMBER [Measures].[Inventory] as iif (ISEMPTY([Measures].[Inventory Count]),0, [Measures].[Inventory Count]) 

                        SELECT   {
                        [Measures].[Listed/Delisted]
                        ,[Measures].[9L Eq Cases CP TY]
                        ,[Measures].[9L Eq Cases CP LY]
                        ,[Measures].[9L Eq Cases CP LY Var %]
                        ,[Measures].[Sales Amount CP TY]
                        ,[Measures].[Sales Amount CP LY]
                        ,[Measures].[Sales Amount CP LY Var %]
                        ,[Measures].[Units CP TY]
                        ,[Measures].[Units CP LY]
                        ,[Measures].[Units CP LY Var %]
                        ,[Measures].[9L Eq Cases P6P]
                        ,[Measures].[9L Eq Cases Prior P6P]
                        ,[Measures].[9L Eq Cases Prior P6P Var %]
                        ,[Measures].[Sales Amount P6P]
                        ,[Measures].[Sales Amount Prior P6P]
                        ,[Measures].[Sales Amount Prior P6P Var %]
                        ,[Measures].[Units P6P]
                        ,[Measures].[Units Prior P6P]
                        ,[Measures].[Units Prior P6P Var %]
                        ,[Measures].[9L Eq Cases MAT]
                        ,[Measures].[9L Eq Cases Prior MAT]
                        ,[Measures].[9L Eq Cases Prior MAT Var %]
                        ,[Measures].[Sales Amount MAT]
                        ,[Measures].[Sales Amount Prior MAT]
                        ,[Measures].[Sales Amount Prior MAT Var %]
                        ,[Measures].[Units MAT]
                        ,[Measures].[Units Prior MAT]
                        ,[Measures].[Units Prior MAT Var %]  
                        ,[Measures].[Promo List]
                        ,[Measures].[Inventory]
                        }
                        ON COLUMNS
                        , NON EMPTY
                        FILTER(
                                --[Product].[Product Name].[Product Name].members
                                --*[Product].[Volume Per Unit ML].[Volume Per Unit ML].members
                                @AccountNumber
                                , IIF(ISEMPTY([Measures].[Sales Amount Prior MAT Var %]), 'a', [Measures].[Sales Amount Prior MAT Var %])
	                    <>'a')
                        ON ROWS
                        FROM [LCBO Week]
                        where
                        (
                        [Account].[Channel].&[OFF TRADE]
                        , @Period
                        , [Product].[Is Own].&[1] -- Pelham Only
                        //, @UserId -- parameter from prev.page
                        --, @AccountNumber -- parameter from prev.page
                        )
        ";

        public const string ProductsNotInStoreLicensee = @"
                        with 
                        set [WarehouseSet] as  {filter ([Account].[Account Name].[Account Name].members,[Account].[Region].&[9])} 

                        select
                        non empty
                        [Measures].[Inventory Count] * (
                        filter([Product].[Product Name].[Product Name].members, iif(   isempty([Measures].[Inventory Count])=true,0,[Measures].[Inventory Count] >0))
                        - 
                        filter( [Product].[Product Name].[Product Name].members 
                                      ,@Period--param
                                      and @AccountNumber --param  
                                      and [Product].[Is Own].&[1] 
                                      )
                        ) * [Product].[CSPC].[CSPC].members on rows 
                        ,  [WarehouseSet]   on columns
                        from
                        [LCBO Week]
                        where  
                        (  
                         @Period --param
                        ,[Product].[Is Own].&[1]

                        )
        ";

        public const string LicenseeDetails = @"
                        WITH
                        MEMBER [Measures].[LCBO Sales CP TY] as ([Measures].[Sales Amount CP TY], [Feed].[Feed].&[LCBO], @Period)
                        MEMBER [Measures].[Direct Sales CP TY] as ([Measures].[Sales Amount CP TY], [Feed].[Feed].&[DIRECT], @Period)
                        MEMBER [Measures].[LCBO Sales 1P TY] as ([Measures].[Sales Amount CP TY], [Feed].[Feed].&[LCBO], @Period.lag(1))
                        MEMBER [Measures].[Direct Sales 1P TY] as ([Measures].[Sales Amount CP TY], [Feed].[Feed].&[DIRECT], @Period.lag(1))
                        MEMBER [Measures].[LCBO Sales 2P TY] as ([Measures].[Sales Amount CP TY], [Feed].[Feed].&[LCBO], @Period.lag(2))
                        MEMBER [Measures].[Direct Sales 2P TY] as ([Measures].[Sales Amount CP TY], [Feed].[Feed].&[DIRECT], @Period.lag(2))
                        MEMBER [Measures].[LCBO Sales 3P TY] as ([Measures].[Sales Amount CP TY], [Feed].[Feed].&[LCBO], @Period.lag(3))
                        MEMBER [Measures].[Direct Sales 3P TY] as ([Measures].[Sales Amount CP TY], [Feed].[Feed].&[DIRECT], @Period.lag(3))
                        MEMBER [Measures].[LCBO Sales 4P TY] as ([Measures].[Sales Amount CP TY], [Feed].[Feed].&[LCBO], @Period.lag(4))
                        MEMBER [Measures].[Direct Sales 4P TY] as ([Measures].[Sales Amount CP TY], [Feed].[Feed].&[DIRECT], @Period.lag(4))
                        MEMBER [Measures].[LCBO Sales 5P TY] as ([Measures].[Sales Amount CP TY], [Feed].[Feed].&[LCBO], @Period.lag(5))
                        MEMBER [Measures].[Direct Sales 5P TY] as ([Measures].[Sales Amount CP TY], [Feed].[Feed].&[DIRECT], @Period.lag(5))
                        MEMBER [Measures].[LCBO Sales 6P TY] as ([Measures].[Sales Amount CP TY], [Feed].[Feed].&[LCBO], @Period.lag(6))
                        MEMBER [Measures].[Direct Sales 6P TY] as ([Measures].[Sales Amount CP TY], [Feed].[Feed].&[DIRECT], @Period.lag(6))
                        MEMBER [Measures].[LCBO Sales 7P TY] as ([Measures].[Sales Amount CP TY], [Feed].[Feed].&[LCBO], @Period.lag(7))
                        MEMBER [Measures].[Direct Sales 7P TY] as ([Measures].[Sales Amount CP TY], [Feed].[Feed].&[DIRECT], @Period.lag(7))
                        MEMBER [Measures].[LCBO Sales 8P TY] as ([Measures].[Sales Amount CP TY], [Feed].[Feed].&[LCBO], @Period.lag(8))
                        MEMBER [Measures].[Direct Sales 8P TY] as ([Measures].[Sales Amount CP TY], [Feed].[Feed].&[DIRECT], @Period.lag(8))
                        MEMBER [Measures].[LCBO Sales 9P TY] as ([Measures].[Sales Amount CP TY], [Feed].[Feed].&[LCBO], @Period.lag(9))
                        MEMBER [Measures].[Direct Sales 9P TY] as ([Measures].[Sales Amount CP TY], [Feed].[Feed].&[DIRECT], @Period.lag(9))
                        MEMBER [Measures].[LCBO Sales 10P TY] as ([Measures].[Sales Amount CP TY], [Feed].[Feed].&[LCBO], @Period.lag(10))
                        MEMBER [Measures].[Direct Sales 10P TY] as ([Measures].[Sales Amount CP TY], [Feed].[Feed].&[DIRECT], @Period.lag(10))
                        MEMBER [Measures].[LCBO Sales 11P TY] as ([Measures].[Sales Amount CP TY], [Feed].[Feed].&[LCBO], @Period.lag(11))
                        MEMBER [Measures].[Direct Sales 11P TY] as ([Measures].[Sales Amount CP TY], [Feed].[Feed].&[DIRECT], @Period.lag(11))
                        MEMBER [Measures].[LCBO Sales 12P TY] as ([Measures].[Sales Amount CP TY], [Feed].[Feed].&[LCBO], @Period.lag(12))
                        MEMBER [Measures].[Direct Sales 12P TY] as ([Measures].[Sales Amount CP TY], [Feed].[Feed].&[DIRECT], @Period.lag(12))
                        SELECT
                        {
		                          [Measures].[LCBO Sales CP TY]
		                        , [Measures].[Direct Sales CP TY]
		                        , [Measures].[LCBO Sales 1P TY]
		                        , [Measures].[Direct Sales 1P TY]
		                        , [Measures].[LCBO Sales 2P TY]
		                        , [Measures].[Direct Sales 2P TY]
		                        , [Measures].[LCBO Sales 3P TY]
		                        , [Measures].[Direct Sales 3P TY]
		                        , [Measures].[LCBO Sales 4P TY]
		                        , [Measures].[Direct Sales 4P TY]
		                        , [Measures].[LCBO Sales 5P TY]
		                        , [Measures].[Direct Sales 5P TY]
		                        , [Measures].[LCBO Sales 6P TY]
		                        , [Measures].[Direct Sales 6P TY]
		                        , [Measures].[LCBO Sales 7P TY]
		                        , [Measures].[Direct Sales 7P TY]
		                        , [Measures].[LCBO Sales 8P TY]
		                        , [Measures].[Direct Sales 8P TY]
		                        , [Measures].[LCBO Sales 9P TY]
		                        , [Measures].[Direct Sales 9P TY]
		                        , [Measures].[LCBO Sales 10P TY]
		                        , [Measures].[Direct Sales 10P TY]
		                        , [Measures].[LCBO Sales 11P TY]
		                        , [Measures].[Direct Sales 11P TY]
		                        , [Measures].[LCBO Sales 12P TY]
		                        , [Measures].[Direct Sales 12P TY]
                        } ON COLUMNS 
                        , NON EMPTY
                        {
                        [Product].[My Category].[My Category].members
                        * [Product].[CSPC].[CSPC].members
                        * [Product].[Product Name].[Product Name].members
                        * [Product].[Volume Per Unit ML].[Volume Per Unit ML].members
                        * [Product].[Price].[Price].members
                        }
                        ON ROWS 
                        FROM  
                        [LCBO Week]
                        where  
                        (
                        [Account].[Channel].&[ON TRADE]
                        , @AccountName
                        )
        ";

        public const string Search = @"
                        WITH MEMBER [Measures].[Terr No] AS [Sales Rep].[Sales Rep].member_key

                        SELECT   {[Measures].[Sales Amount]}  ON COLUMNS 
                        , NON EMPTY 
						Filter(			
						[Account].[Account Name].[Account Name]
                        *[Account].[City].[City]
                        *[Account].[Address].[Address]
                        *[Account].[Postal Code].[Postal Code]
                        *[Account].[Region].[Region]
                        *[Account].[Account Number].[Account Number]
						, NOT ISEMPTY([Measures].[Sales Amount] ))
                        ON ROWS              
                        FROM  [LCBO Week]
                        where  (
                        [Account].[Channel].&[OFF TRADE]                   
                        )
        ";
        public const string LicSearch = @"
                        WITH MEMBER [Measures].[Terr No] AS [Sales Rep].[Sales Rep].member_key

                        SELECT   {[Measures].[Sales Amount]}  ON COLUMNS 
                        , NON EMPTY 
						Filter(			
						
						[Account].[Account Name].[Account Name]
                        *[Account].[City].[City]
                        *[Account].[Address].[Address]
                        *[Account].[Postal Code].[Postal Code]
                        *[Account].[Region].[Region]
                        *[Account].[Account Number].[Account Number]
						, NOT ISEMPTY([Measures].[Sales Amount] ))
                        ON ROWS              
                        FROM  [LCBO Week]
                        where  
                        (
                        [Account].[Channel].&[ON TRADE]                   
                        )
        ";

        public const string CSPCSearch = @"                        
						WITH MEMBER [Measures].[S] AS(1)
                        SELECT   { [Measures].[S] }  ON COLUMNS 
                        , NON EMPTY 
						Filter(			
						NONEMPTY([Product].[CSPC].[CSPC])
						* [Product].[Product Name].[Product Name]	
						* [Product].[Category].[Category]
						* [Product].[Subcategory].[Subcategory]
						*	[Product].[Brand].[Brand]		
						, NOT ISEMPTY( [Product].[Product Name].[Product Name] ))
                        ON ROWS              
                        FROM  [LCBO Week]
        ";

        public const string CSPCStore = @"
                        WITH
                        SET [SalesAmountSet] AS Order (filter( [Account].[Account Number].[Account Number].members, iif (isempty([Measures].[Sales Amount Prior MAT Var %]),'a', [Measures].[Sales Amount Prior MAT Var %])<>'a' ),[Measures].[Sales Amount MAT],BDESC)
                        MEMBER [Measures].[Rank Sales Amount] as Rank( [Account].[Account Number].CurrentMember , [SalesAmountSet]) 

                        SET [UnitsSet] AS Order (filter( [Account].[Account Number].[Account Number].members, iif (isempty([Measures].[Sales Amount Prior MAT Var %]),'a', [Measures].[Sales Amount Prior MAT Var %])<>'a' ),[Measures].[Units MAT],BDESC)
                        MEMBER [Measures].[Rank Units] as Rank( [Account].[Account Number].CurrentMember , [UnitsSet])

                        SET [9LEqCasesSet] AS Order (filter( [Account].[Account Number].[Account Number].members, iif (isempty([Measures].[Sales Amount Prior MAT Var %]),'a', [Measures].[Sales Amount Prior MAT Var %])<>'a' ),[Measures].[Measures].[9L Eq Cases MAT],BDESC)
                        MEMBER [Measures].[Rank 9L Eq Cases MAT] as Rank( [Account].[Account Number].CurrentMember , [9LEqCasesSet]) 

                        MEMBER [Measures].[9L 13P Direct Sales] as ([Measures].[9L Eq Cases MAT], [Feed].[Feed].&[DIRECT])
                        MEMBER [Measures].[9L 13P Direct Sales Prior] as ([Measures].[9L Eq Cases Prior MAT], [Feed].[Feed].&[DIRECT])
                        MEMBER [Measures].[9L 13P Direct Sales Prior Var] as ([Measures].[9L Eq Cases Prior MAT Var %], [Feed].[Feed].&[DIRECT])
                        MEMBER [Measures].[Sales 13P Direct Sales] as ([Measures].[Sales Amount MAT], [Feed].[Feed].&[DIRECT])
                        MEMBER [Measures].[Sales 13P Direct Sales Prior] as ([Measures].[Sales Amount Prior MAT], [Feed].[Feed].&[DIRECT])
                        MEMBER [Measures].[Sales 13P Direct Sales Prior Var] as ([Measures].[Sales Amount Prior MAT Var %], [Feed].[Feed].&[DIRECT])
                        MEMBER [Measures].[Units 13P Direct Sales] as ([Measures].[Units MAT], [Feed].[Feed].&[DIRECT])
                        MEMBER [Measures].[Units 13P Direct Sales Prior] as ([Measures].[Units Prior MAT], [Feed].[Feed].&[DIRECT])
                        MEMBER [Measures].[Units 13P Direct Sales Prior Var] as ([Measures].[Units Prior MAT Var %], [Feed].[Feed].&[DIRECT])

                        MEMBER [Measures].[9L 3P Direct Sales] as ([Measures].[9L Eq Cases P3P], [Feed].[Feed].&[DIRECT])
                        MEMBER [Measures].[9L 3P Direct Sales Prior] as ([Measures].[9L Eq Cases Prior P3P], [Feed].[Feed].&[DIRECT])
                        MEMBER [Measures].[9L 3P Direct Sales Prior Var] as ([Measures].[9L Eq Cases Prior P3P Var %], [Feed].[Feed].&[DIRECT])
                        MEMBER [Measures].[Sales 3P Direct Sales] as ([Measures].[Sales Amount P3P], [Feed].[Feed].&[DIRECT])
                        MEMBER [Measures].[Sales 3P Direct Sales Prior] as ([Measures].[Sales Amount Prior P3P], [Feed].[Feed].&[DIRECT])
                        MEMBER [Measures].[Sales 3P Direct Sales Prior Var] as ([Measures].[Sales Amount Prior P3P Var %], [Feed].[Feed].&[DIRECT])
                        MEMBER [Measures].[Units 3P Direct Sales] as ([Measures].[Units P3P], [Feed].[Feed].&[DIRECT])
                        MEMBER [Measures].[Units 3P Direct Sales Prior] as ([Measures].[Units Prior P3P], [Feed].[Feed].&[DIRECT])
                        MEMBER [Measures].[Units 3P Direct Sales Prior Var] as ([Measures].[Units Prior P3P Var %], [Feed].[Feed].&[DIRECT])

                        MEMBER [Measures].[9L 13P Licensee] as ([Measures].[9L Eq Cases MAT], [Feed].[Feed].&[LCBO])
                        MEMBER [Measures].[9L 13P Licensee Prior] as ([Measures].[9L Eq Cases Prior MAT], [Feed].[Feed].&[LCBO])
                        MEMBER [Measures].[9L 13P Licensee Prior Var] as ([Measures].[9L Eq Cases Prior MAT Var %], [Feed].[Feed].&[LCBO])
                        MEMBER [Measures].[Sales 13P Licensee] as ([Measures].[Sales Amount MAT], [Feed].[Feed].&[LCBO])
                        MEMBER [Measures].[Sales 13P Licensee Prior] as ([Measures].[Sales Amount Prior MAT], [Feed].[Feed].&[LCBO])
                        MEMBER [Measures].[Sales 13P Licensee Prior Var] as ([Measures].[Sales Amount Prior MAT Var %], [Feed].[Feed].&[LCBO])
                        MEMBER [Measures].[Units 13P Licensee] as ([Measures].[Units MAT], [Feed].[Feed].&[LCBO])
                        MEMBER [Measures].[Units 13P Licensee Prior] as ([Measures].[Units Prior MAT], [Feed].[Feed].&[LCBO])
                        MEMBER [Measures].[Units 13P Licensee Prior Var] as ([Measures].[Units Prior MAT Var %], [Feed].[Feed].&[LCBO])

                        MEMBER [Measures].[9L 3P Licensee] as ([Measures].[9L Eq Cases P3P], [Feed].[Feed].&[LCBO])
                        MEMBER [Measures].[9L 3P Licensee Prior] as ([Measures].[9L Eq Cases Prior P3P], [Feed].[Feed].&[LCBO])
                        MEMBER [Measures].[9L 3P Licensee Prior Var] as ([Measures].[9L Eq Cases Prior P3P Var %], [Feed].[Feed].&[LCBO])
                        MEMBER [Measures].[Sales 3P Licensee] as ([Measures].[Sales Amount P3P], [Feed].[Feed].&[LCBO])
                        MEMBER [Measures].[Sales 3P Licensee Prior] as ([Measures].[Sales Amount Prior P3P], [Feed].[Feed].&[LCBO])
                        MEMBER [Measures].[Sales 3P Licensee Prior Var] as ([Measures].[Sales Amount Prior P3P Var %], [Feed].[Feed].&[LCBO])
                        MEMBER [Measures].[Units 3P Licensee] as ([Measures].[Units P3P], [Feed].[Feed].&[LCBO])
                        MEMBER [Measures].[Units 3P Licensee Prior] as ([Measures].[Units Prior P3P], [Feed].[Feed].&[LCBO])
                        MEMBER [Measures].[Units 3P Licensee Prior Var] as ([Measures].[Units Prior P3P Var %], [Feed].[Feed].&[LCBO])

                        SELECT
                        {
                        [Measures].[Rank Sales Amount] 
                        ,[Measures].[Rank Units]
                        ,[Measures].[Rank 9L Eq Cases MAT]

                        ,[Measures].[9L Eq Cases MAT]
                        ,[Measures].[9L Eq Cases Prior MAT]
                        ,[Measures].[9L Eq Cases Prior MAT Var %]
                        ,[Measures].[Sales Amount MAT]
                        ,[Measures].[Sales Amount Prior MAT]
                        ,[Measures].[Sales Amount Prior MAT Var %]
                        ,[Measures].[Units MAT]
                        ,[Measures].[Units Prior MAT]
                        ,[Measures].[Units Prior MAT Var %]       
                        ,[Measures].[9L Eq Cases P3P]
                        ,[Measures].[9L Eq Cases Prior P3P]
                        ,[Measures].[9L Eq Cases Prior P3P Var %]
                        ,[Measures].[Sales Amount P3P]
                        ,[Measures].[Sales Amount Prior P3P]
                        ,[Measures].[Sales Amount Prior P3P Var %]
                        ,[Measures].[Units P3P]
                        ,[Measures].[Units Prior P3P]
                        ,[Measures].[Units Prior P3P Var %]

                        ,[Measures].[9L 13P Direct Sales]
                        ,[Measures].[9L 13P Direct Sales Prior]
                        ,[Measures].[9L 13P Direct Sales Prior Var]
                        ,[Measures].[Sales 13P Direct Sales]
                        ,[Measures].[Sales 13P Direct Sales Prior]
                        ,[Measures].[Sales 13P Direct Sales Prior Var]
                        ,[Measures].[Units 13P Direct Sales]
                        ,[Measures].[Units 13P Direct Sales Prior]
                        ,[Measures].[Units 13P Direct Sales Prior Var]

                        ,[Measures].[9L 3P Direct Sales]
                        ,[Measures].[9L 3P Direct Sales Prior]
                        ,[Measures].[9L 3P Direct Sales Prior Var]
                        ,[Measures].[Sales 3P Direct Sales]
                        ,[Measures].[Sales 3P Direct Sales Prior]
                        ,[Measures].[Sales 3P Direct Sales Prior Var]
                        ,[Measures].[Units 3P Direct Sales]
                        ,[Measures].[Units 3P Direct Sales Prior]
                        ,[Measures].[Units 3P Direct Sales Prior Var]

                        ,[Measures].[9L 13P Licensee]
                        ,[Measures].[9L 13P Licensee Prior]
                        ,[Measures].[9L 13P Licensee Prior Var]
                        ,[Measures].[Sales 13P Licensee]
                        ,[Measures].[Sales 13P Licensee Prior]
                        ,[Measures].[Sales 13P Licensee Prior Var]
                        ,[Measures].[Units 13P Licensee]
                        ,[Measures].[Units 13P Licensee Prior]
                        ,[Measures].[Units 13P Licensee Prior Var]

                        ,[Measures].[9L 3P Licensee]
                        ,[Measures].[9L 3P Licensee Prior]
                        ,[Measures].[9L 3P Licensee Prior Var]
                        ,[Measures].[Sales 3P Licensee]
                        ,[Measures].[Sales 3P Licensee Prior]
                        ,[Measures].[Sales 3P Licensee Prior Var]
                        ,[Measures].[Units 3P Licensee]
                        ,[Measures].[Units 3P Licensee Prior]
                        ,[Measures].[Units 3P Licensee Prior Var]
                        }
                        ON COLUMNS  
                        , NON EMPTY 
                        ORDER(  Filter(
                                [Account].[Account Number].[Account Number].members
                                * [Account].[Account Name].[Account Name].members
                                * [Account].[City].[City].members
                                * [Account].[Address].[Address].MEMBERS
                           ,[Measures].[Rank Sales Amount]>0)
                            ,[Measures].[Rank Sales Amount], BASC
                        )
                        ON ROWS              
                        FROM
                        [LCBO Week]
                        WHERE
                        (
                        [Account].[Channel].&[OFF TRADE], --Store
                        @Period,
                        //, [Product].[Is Own].&[1] -- Pelham Only
                        @CSPC
                        )
        ";

        public const string CSPCLicensee = @"
                        WITH
                        SET [SalesAmountSet] AS Order (filter( [Account].[Account Number].[Account Number].members, iif (isempty([Measures].[Sales Amount Prior MAT Var %]),'a', [Measures].[Sales Amount Prior MAT Var %])<>'a' ),[Measures].[Sales Amount MAT],BDESC)
                        MEMBER [Measures].[Rank Sales Amount] as Rank( [Account].[Account Number].CurrentMember , [SalesAmountSet]) 

                        SET [UnitsSet] AS Order (filter( [Account].[Account Number].[Account Number].members, iif (isempty([Measures].[Sales Amount Prior MAT Var %]),'a', [Measures].[Sales Amount Prior MAT Var %])<>'a' ),[Measures].[Units MAT],BDESC)
                        MEMBER [Measures].[Rank Units] as Rank( [Account].[Account Number].CurrentMember , [UnitsSet])

                        SET [9LEqCasesSet] AS Order (filter( [Account].[Account Number].[Account Number].members, iif (isempty([Measures].[Sales Amount Prior MAT Var %]),'a', [Measures].[Sales Amount Prior MAT Var %])<>'a' ),[Measures].[Measures].[9L Eq Cases MAT],BDESC)
                        MEMBER [Measures].[Rank 9L Eq Cases MAT] as Rank( [Account].[Account Number].CurrentMember , [9LEqCasesSet]) 

                        MEMBER [Measures].[9L 13P Direct Sales] as ([Measures].[9L Eq Cases MAT], [Feed].[Feed].&[DIRECT])
                        MEMBER [Measures].[9L 13P Direct Sales Prior] as ([Measures].[9L Eq Cases Prior MAT], [Feed].[Feed].&[DIRECT])
                        MEMBER [Measures].[9L 13P Direct Sales Prior Var] as ([Measures].[9L Eq Cases Prior MAT Var %], [Feed].[Feed].&[DIRECT])
                        MEMBER [Measures].[Sales 13P Direct Sales] as ([Measures].[Sales Amount MAT], [Feed].[Feed].&[DIRECT])
                        MEMBER [Measures].[Sales 13P Direct Sales Prior] as ([Measures].[Sales Amount Prior MAT], [Feed].[Feed].&[DIRECT])
                        MEMBER [Measures].[Sales 13P Direct Sales Prior Var] as ([Measures].[Sales Amount Prior MAT Var %], [Feed].[Feed].&[DIRECT])
                        MEMBER [Measures].[Units 13P Direct Sales] as ([Measures].[Units MAT], [Feed].[Feed].&[DIRECT])
                        MEMBER [Measures].[Units 13P Direct Sales Prior] as ([Measures].[Units Prior MAT], [Feed].[Feed].&[DIRECT])
                        MEMBER [Measures].[Units 13P Direct Sales Prior Var] as ([Measures].[Units Prior MAT Var %], [Feed].[Feed].&[DIRECT])

                        MEMBER [Measures].[9L 3P Direct Sales] as ([Measures].[9L Eq Cases P3P], [Feed].[Feed].&[DIRECT])
                        MEMBER [Measures].[9L 3P Direct Sales Prior] as ([Measures].[9L Eq Cases Prior P3P], [Feed].[Feed].&[DIRECT])
                        MEMBER [Measures].[9L 3P Direct Sales Prior Var] as ([Measures].[9L Eq Cases Prior P3P Var %], [Feed].[Feed].&[DIRECT])
                        MEMBER [Measures].[Sales 3P Direct Sales] as ([Measures].[Sales Amount P3P], [Feed].[Feed].&[DIRECT])
                        MEMBER [Measures].[Sales 3P Direct Sales Prior] as ([Measures].[Sales Amount Prior P3P], [Feed].[Feed].&[DIRECT])
                        MEMBER [Measures].[Sales 3P Direct Sales Prior Var] as ([Measures].[Sales Amount Prior P3P Var %], [Feed].[Feed].&[DIRECT])
                        MEMBER [Measures].[Units 3P Direct Sales] as ([Measures].[Units P3P], [Feed].[Feed].&[DIRECT])
                        MEMBER [Measures].[Units 3P Direct Sales Prior] as ([Measures].[Units Prior P3P], [Feed].[Feed].&[DIRECT])
                        MEMBER [Measures].[Units 3P Direct Sales Prior Var] as ([Measures].[Units Prior P3P Var %], [Feed].[Feed].&[DIRECT])

                        MEMBER [Measures].[9L 13P Licensee] as ([Measures].[9L Eq Cases MAT], [Feed].[Feed].&[LCBO])
                        MEMBER [Measures].[9L 13P Licensee Prior] as ([Measures].[9L Eq Cases Prior MAT], [Feed].[Feed].&[LCBO])
                        MEMBER [Measures].[9L 13P Licensee Prior Var] as ([Measures].[9L Eq Cases Prior MAT Var %], [Feed].[Feed].&[LCBO])
                        MEMBER [Measures].[Sales 13P Licensee] as ([Measures].[Sales Amount MAT], [Feed].[Feed].&[LCBO])
                        MEMBER [Measures].[Sales 13P Licensee Prior] as ([Measures].[Sales Amount Prior MAT], [Feed].[Feed].&[LCBO])
                        MEMBER [Measures].[Sales 13P Licensee Prior Var] as ([Measures].[Sales Amount Prior MAT Var %], [Feed].[Feed].&[LCBO])
                        MEMBER [Measures].[Units 13P Licensee] as ([Measures].[Units MAT], [Feed].[Feed].&[LCBO])
                        MEMBER [Measures].[Units 13P Licensee Prior] as ([Measures].[Units Prior MAT], [Feed].[Feed].&[LCBO])
                        MEMBER [Measures].[Units 13P Licensee Prior Var] as ([Measures].[Units Prior MAT Var %], [Feed].[Feed].&[LCBO])

                        MEMBER [Measures].[9L 3P Licensee] as ([Measures].[9L Eq Cases P3P], [Feed].[Feed].&[LCBO])
                        MEMBER [Measures].[9L 3P Licensee Prior] as ([Measures].[9L Eq Cases Prior P3P], [Feed].[Feed].&[LCBO])
                        MEMBER [Measures].[9L 3P Licensee Prior Var] as ([Measures].[9L Eq Cases Prior P3P Var %], [Feed].[Feed].&[LCBO])
                        MEMBER [Measures].[Sales 3P Licensee] as ([Measures].[Sales Amount P3P], [Feed].[Feed].&[LCBO])
                        MEMBER [Measures].[Sales 3P Licensee Prior] as ([Measures].[Sales Amount Prior P3P], [Feed].[Feed].&[LCBO])
                        MEMBER [Measures].[Sales 3P Licensee Prior Var] as ([Measures].[Sales Amount Prior P3P Var %], [Feed].[Feed].&[LCBO])
                        MEMBER [Measures].[Units 3P Licensee] as ([Measures].[Units P3P], [Feed].[Feed].&[LCBO])
                        MEMBER [Measures].[Units 3P Licensee Prior] as ([Measures].[Units Prior P3P], [Feed].[Feed].&[LCBO])
                        MEMBER [Measures].[Units 3P Licensee Prior Var] as ([Measures].[Units Prior P3P Var %], [Feed].[Feed].&[LCBO])

                        SELECT
                        {
                        [Measures].[Rank Sales Amount] 
                        ,[Measures].[Rank Units]
                        ,[Measures].[Rank 9L Eq Cases MAT]

                        ,[Measures].[9L Eq Cases MAT]
                        ,[Measures].[9L Eq Cases Prior MAT]
                        ,[Measures].[9L Eq Cases Prior MAT Var %]
                        ,[Measures].[Sales Amount MAT]
                        ,[Measures].[Sales Amount Prior MAT]
                        ,[Measures].[Sales Amount Prior MAT Var %]
                        ,[Measures].[Units MAT]
                        ,[Measures].[Units Prior MAT]
                        ,[Measures].[Units Prior MAT Var %]       
                        ,[Measures].[9L Eq Cases P3P]
                        ,[Measures].[9L Eq Cases Prior P3P]
                        ,[Measures].[9L Eq Cases Prior P3P Var %]
                        ,[Measures].[Sales Amount P3P]
                        ,[Measures].[Sales Amount Prior P3P]
                        ,[Measures].[Sales Amount Prior P3P Var %]
                        ,[Measures].[Units P3P]
                        ,[Measures].[Units Prior P3P]
                        ,[Measures].[Units Prior P3P Var %]

                        ,[Measures].[9L 13P Direct Sales]
                        ,[Measures].[9L 13P Direct Sales Prior]
                        ,[Measures].[9L 13P Direct Sales Prior Var]
                        ,[Measures].[Sales 13P Direct Sales]
                        ,[Measures].[Sales 13P Direct Sales Prior]
                        ,[Measures].[Sales 13P Direct Sales Prior Var]
                        ,[Measures].[Units 13P Direct Sales]
                        ,[Measures].[Units 13P Direct Sales Prior]
                        ,[Measures].[Units 13P Direct Sales Prior Var]

                        ,[Measures].[9L 3P Direct Sales]
                        ,[Measures].[9L 3P Direct Sales Prior]
                        ,[Measures].[9L 3P Direct Sales Prior Var]
                        ,[Measures].[Sales 3P Direct Sales]
                        ,[Measures].[Sales 3P Direct Sales Prior]
                        ,[Measures].[Sales 3P Direct Sales Prior Var]
                        ,[Measures].[Units 3P Direct Sales]
                        ,[Measures].[Units 3P Direct Sales Prior]
                        ,[Measures].[Units 3P Direct Sales Prior Var]

                        ,[Measures].[9L 13P Licensee]
                        ,[Measures].[9L 13P Licensee Prior]
                        ,[Measures].[9L 13P Licensee Prior Var]
                        ,[Measures].[Sales 13P Licensee]
                        ,[Measures].[Sales 13P Licensee Prior]
                        ,[Measures].[Sales 13P Licensee Prior Var]
                        ,[Measures].[Units 13P Licensee]
                        ,[Measures].[Units 13P Licensee Prior]
                        ,[Measures].[Units 13P Licensee Prior Var]

                        ,[Measures].[9L 3P Licensee]
                        ,[Measures].[9L 3P Licensee Prior]
                        ,[Measures].[9L 3P Licensee Prior Var]
                        ,[Measures].[Sales 3P Licensee]
                        ,[Measures].[Sales 3P Licensee Prior]
                        ,[Measures].[Sales 3P Licensee Prior Var]
                        ,[Measures].[Units 3P Licensee]
                        ,[Measures].[Units 3P Licensee Prior]
                        ,[Measures].[Units 3P Licensee Prior Var]
                        }
                        ON COLUMNS  
                        , NON EMPTY 
                        ORDER(  Filter(
                                [Account].[Account Number].[Account Number].members
                                * [Account].[Account Name].[Account Name].members
                                * [Account].[City].[City].members
                                * [Account].[Address].[Address].MEMBERS
                           ,[Measures].[Rank Sales Amount]>0)
                            ,[Measures].[Rank Sales Amount], BASC
                        )
                        ON ROWS              
                        FROM
                        [LCBO Week]
                        WHERE
                        (
                        [Account].[Channel].&[ON TRADE], --Licensee
                        @Period,
                        //, [Product].[Is Own].&[1] -- Pelham Only
                        @CSPC
                        )
        ";

        public const string StoreSalesPromotionCode = @"
                    SELECT
                    {
                        [Measures].[Promotion Count]      
                    } ON COLUMNS 
                    , NON EMPTY
                    [Product].[CSPC].[CSPC].members
                    --* [Product].[Product Name].[Product Name].members
                    --* [Product].[Volume Per Unit ML].[Volume Per Unit ML].members
                    * [Promotion].[Promotion Code].[Promotion Code].members
                    ON ROWS 
                    FROM  
                    [LCBO Week]
                    where  
                    (
                        [Account].[Channel].&[OFF TRADE]
                        , @Period
                        --, [Product].[Category].&[SPARKLING]
                        ,@AccountNumber
                    ) 
        ";
    }
}
