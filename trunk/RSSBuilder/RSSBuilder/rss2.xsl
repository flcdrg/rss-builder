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
                    html { border: 8px solid gray; }
                    body { font: 84% Verdana, Geneva, Arial, Helvetica, sans-serif; margin: 20px; }
                    h1 { font-size: 120%; }
                    h2 { font-size: 100%; }
                    div.newsitem {  border-top: 1px dotted silver; margin: 20px; }
                    .rssnote {
                        font-style: italic;
                        color: gray;
                        text-align: center;
                        border: 1px solid gray;
                        background-color: #EDEDED;
                        padding: 4px 8px;
                    }
                    .newsitemcontent {}
                    .newsitemfooter {   font-style: italic; font-size: xx-small; color: gray; text-align: right; }
                    
                    A:link { text-decoration: none; color: #551DC6;}
                    A:visited { text-decoration: none; color: #666666; }
                    A:hover {text-decoration: underline; }                 
                    A:active { text-decoration: none; color: #551DC6; }

                </style>
        </head>           
        
        <body>
            
            <!-- Display the tile, the image and the description of the RSS feed
             -->
             
            <center>                
                <h1>
                    <a href="{link}" target="_blank">
                       <xsl:value-of select="title" />
                    </a>   
                </h1>                            
                
                <xsl:if test="image">
                   <!-- Selecteer sub-element URL van element IMAGE -->                 
                   <img src="{image/url}" />                    
                    <br />
                </xsl:if>
                
                <xsl:value-of select="description" />
                <br />                          
                
                <p class="rssnote">
                   This page is meant to be displayed using an RSS reader. 
                </p>

            </center>
            
            <!-- Display all news feed items
             -->
            
            <xsl:for-each select="item"> 
            
                <div class="newsitem">
            
               
                <!-- Display the feed title -->
               
                <h2><a href="{link}" target="_blank">                
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
                  <li />Enclosure: 
                  <a href="{enclosure/@url}" target="_blank">                               
                    <xsl:value-of select="enclosure/@url" />
                  </a>
                  (<xsl:value-of select="enclosure/@type" />,
                  <xsl:value-of select="enclosure/@length" /> bytes)                              
                  
                </xsl:if>

                <!-- Display author and publication date
                 -->
                 
                <p class="newsitemfooter">
                <xsl:choose>
                    <xsl:when test="author">
                        <xsl:value-of select="author" /> - <xsl:value-of select="pubDate" />
                    </xsl:when>
                    <xsl:otherwise>
                       <xsl:if test="pubDate">
                           <xsl:value-of select="pubDate" />
                       </xsl:if>
                    </xsl:otherwise>
                </xsl:choose>
                </p>
                
                </div>
            </xsl:for-each>  
            
            
            <!-- Display the footer (copyright)
             -->
            
            <span style="float:right">
                <small><xsl:value-of select="copyright" /></small>
            </span>
            
        </body>
    </html>
    </xsl:template>
</xsl:stylesheet>
