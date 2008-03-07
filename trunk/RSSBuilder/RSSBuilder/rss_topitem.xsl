<?xml version="1.0"?>
<!-- used for styling RSS feeds -->
<xsl:stylesheet version="1.0"
    xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
   xmlns:blogChannel="http://backend.userland.com/blogChannelModule">
<xsl:output method="html"/>

<xsl:template match="channel">
    <html>
    <head>
        <title><xsl:value-of select="title" disable-output-escaping="yes"/></title>
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
        </style>
    </head>

    <body>      
        <xsl:for-each select="item[1]">
            <div class="newsitem">

                <xsl:variable name="itemlink" select="link"/>
            <h2><a href="{$itemlink}"><xsl:value-of select="title" disable-output-escaping="yes"/></a></h2>
                <p class="newsitemcontent">
                    <xsl:value-of select="description" disable-output-escaping="yes"/>
                </p>
                <p class="newsitemfooter">
                    <xsl:value-of select="pubDate"/>
                </p>
            </div>

        </xsl:for-each>
    </body>
    </html> 
</xsl:template>
</xsl:stylesheet>