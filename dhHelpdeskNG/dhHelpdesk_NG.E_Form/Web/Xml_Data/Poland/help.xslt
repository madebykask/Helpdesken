<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
    xmlns:msxsl="urn:schemas-microsoft-com:xslt" exclude-result-prefixes="msxsl">

    <xsl:output method="xml"  indent="yes"/>

    <xsl:template match="@* | node()">
        <xsl:copy>
            <xsl:apply-templates select="@* | node()"/>
        </xsl:copy>
    </xsl:template>
    <!-- element -->
    <xsl:template match="/">
        <xsl:for-each select="//@label">
            <data>
                <xsl:attribute name="name">
                    <xsl:value-of select="."/>
                </xsl:attribute>
                <xsl:attribute name="xml:space">
                    <xsl:text>preserve</xsl:text>
                </xsl:attribute>
                <value>
                    <xsl:value-of select="."/>
                </value>
            </data>
        </xsl:for-each>
    </xsl:template>

    <xsl:key name="value" match="//@value" use="." />

    <xsl:template match="/">
        <xsl:for-each select="//@value[generate-id() = generate-id(key('value',.)[1])]">
            <xsl:if test=".!=''">
                <data>
                    <xsl:attribute name="name">
                        <xsl:value-of select="."/>
                    </xsl:attribute>
                    <xsl:attribute name="xml:space">
                        <xsl:text>preserve</xsl:text>
                    </xsl:attribute>
                    <value>
                        <xsl:value-of select="."/>
                    </value>
                </data>
            </xsl:if>
        </xsl:for-each>
    </xsl:template>

</xsl:stylesheet>
