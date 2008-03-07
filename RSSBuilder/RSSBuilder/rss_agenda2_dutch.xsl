<?xml version="1.0" encoding="UTF-8"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
    <xsl:output version="1.0" encoding="utf-8" omit-xml-declaration="no" indent="no" media-type="text/html" />
    
    <!-- link style sheet to element /rss/channel 
     -->
    
    <xsl:template match="/rss/channel">
    <html>
    
        <!-- Generate content as HTML
         -->
         
        <head>
        <meta http-equiv="Content-Type" content="text/html; charset=UTF-8" />
        
        <title><xsl:value-of select="title" /></title>
        
                <style>
                  <!--  html { border: 4px solid blue; } -->
                    body { font: 84% Verdana, Geneva, Arial, Helvetica, sans-serif; margin: 20px; }
                    h1 { font-size: 120%; }
                    h2 { font-size: 100%;  }
                    div.newsitem {  border-bottom: 1px dotted silver; margin: 10px; }
                    .newsitemdate {
                        color: darkblue;
                        text-align: center;
                        border: 1px solid lightblue;
                        background-color: #EFEFEF;
                        padding: 4px 12px;
                    }
                    .newsitemcontent {}
                    .newsitemtitle { border-bottom: 1px; }
                    
                    .newsitemfooter {   font-style: italic; font-size: xx-small; color: gray; text-align: right; }
                    
                    A:link { text-decoration: none; color: #551DC6;}
                    A:visited { text-decoration: none; color: #666666; }
                    A:hover {text-decoration: underline; }                 
                    A:active { text-decoration: none; color: #551DC6; }

                </style>
        </head>           
        
        <body>
            
            <!-- Display some title information
             -->

<!--             
            <center>                
                <p class="rssnote">
                   Agenda 
                </p>
            </center>
-->            
            
            <!-- Display all news feed items
             -->
            
            <xsl:for-each select="item"> 
            
                <div class="newsitem">           
               
                
                   <!-- Display publication date
                    -->

                   <xsl:if test="pubDate">
                       <xsl:variable name="dayName">
                       <xsl:choose>
                          <xsl:when test="starts-with(pubDate, 'Sun')">
                             zondag
                          </xsl:when>   
                          <xsl:when test="starts-with(pubDate, 'Mon')">
                             maandag
                          </xsl:when>   
                          <xsl:when test="starts-with(pubDate, 'Tue')">
                             dinsdag
                          </xsl:when>   
                          <xsl:when test="starts-with(pubDate, 'Wed')">
                             woensdag
                          </xsl:when>   
                          <xsl:when test="starts-with(pubDate, 'Thu')">
                             donderdag
                          </xsl:when>   
                          <xsl:when test="starts-with(pubDate, 'Fri')">
                             vrijdag
                          </xsl:when>   
                          <xsl:when test="starts-with(pubDate, 'Sat')">
                             zaterdag
                          </xsl:when>   
                       </xsl:choose>
                       </xsl:variable>

                       <xsl:variable name="day" select="normalize-space(substring(pubDate, 5,3))"/>

                       <xsl:variable name="month">
                       <xsl:choose>
                          <xsl:when test="contains(pubDate, 'Jan')">
                             januari
                          </xsl:when>   
                          <xsl:when test="contains(pubDate, 'Feb')">
                             februari
                          </xsl:when>   
                          <xsl:when test="contains(pubDate, 'Mar')">
                             maart
                          </xsl:when>   
                          <xsl:when test="contains(pubDate, 'Apr')">
                             april
                          </xsl:when>   
                          <xsl:when test="contains(pubDate, 'May')">
                             mei
                          </xsl:when>   
                          <xsl:when test="contains(pubDate, 'Jun')">
                             juni
                          </xsl:when>   
                          <xsl:when test="contains(pubDate, 'Jul')">
                             juli
                          </xsl:when>   
                          <xsl:when test="contains(pubDate, 'Aug')">
                             augustus
                          </xsl:when>   
                          <xsl:when test="contains(pubDate, 'Sep')">
                             september
                          </xsl:when>   
                          <xsl:when test="contains(pubDate, 'Oct')">
                             oktober
                          </xsl:when>   
                          <xsl:when test="contains(pubDate, 'Nov')">
                             november
                          </xsl:when>   
                          <xsl:when test="contains(pubDate, 'Dec')">
                             december
                          </xsl:when>   
                       </xsl:choose>
                       </xsl:variable>

                       <xsl:variable name="year" select="normalize-space(substring(pubDate, 12,5))"/>

                       <div class="newsitemdate">
                       <xsl:value-of select="concat($dayName, ' ', $day, ' ', $month, ' ', $year)"/>
                       </div>

                   </xsl:if>

                

                <!-- Display the feed title -->
               
                
                <h2>
                <a href="{link}" target="_blank">                
                        <xsl:value-of select="title" />
                </a>
                </h2>


                <!-- Display the content, including HTML (output escaping disabled) 
                 -->

                <p class="newsitemcontent">
                    <xsl:value-of select="description" disable-output-escaping="yes" />
                </p>
                
                <!-- Test for an enclosure and display it
                 -->
                 
                <xsl:if test="enclosure">                                                          
                  <p>
                  <li />Enclosure: 
                  <a href="{enclosure/@url}" target="_blank">                               
                    <xsl:value-of select="enclosure/@url" />
                  </a>
                  (<xsl:value-of select="enclosure/@type" />,
                  <xsl:value-of select="enclosure/@length" /> bytes)
                  </p>
                </xsl:if>

                
                </div>
            </xsl:for-each>  
            
            
            <!-- Display the footer (copyright)
             -->

<!--            
            <span style="float:right">
                <small><xsl:value-of select="copyright" /></small>
            </span>
-->            
            
        </body>
    </html>
    </xsl:template>
</xsl:stylesheet>
