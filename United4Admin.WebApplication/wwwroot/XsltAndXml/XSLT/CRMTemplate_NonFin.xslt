<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
    xmlns:msxsl="urn:schemas-microsoft-com:xslt" exclude-result-prefixes="msxsl">
  <xsl:output method="xml" indent="yes"/>
  <xsl:template match="/">
    <Data>
      <xsl:for-each select="//Echo">
        <Echo>
          <Salutation>
            <xsl:value-of select="Title"/>
          </Salutation>
          <FirstName>
            <xsl:value-of select="Forename"/>
          </FirstName>
          <MiddleName>
            <xsl:value-of select="MiddleName"/>
          </MiddleName>
          <LastName>
            <xsl:value-of select="Surname"/>
          </LastName>
          <Street1>
            <xsl:value-of select="StreetName"/>
          </Street1>
          <PostalCode>
            <xsl:value-of select="Postcode"/>
          </PostalCode>
          <City>
            <xsl:value-of select="AddressLine3"/>
          </City>
          <CountryCode>
            <xsl:value-of select="Country"/>
          </CountryCode>
          <ContactID>
            <xsl:value-of select="supporterID"/>
          </ContactID>
          <LeadID>
            <xsl:value-of select="leadID"/>
          </LeadID>
          <EmailAddress>
            <xsl:value-of select="EmailAddress"/>
          </EmailAddress>
          <PhoneNumber1>
            <xsl:value-of select="PhoneNum1"/>
          </PhoneNumber1>
          <TransactionReferenceNo>
            <xsl:value-of select="TransactionReferenceNo"/>
          </TransactionReferenceNo>
          <Date>
            <xsl:value-of select="DateOfRequest"/>
          </Date>
          <ProductID>
            <xsl:value-of select="ProductID"/>
          </ProductID>
          <DonationvariantID>
            <xsl:value-of select="DonationvariantID"/>
          </DonationvariantID>
          <TriggerCode>
            <xsl:value-of select="TriggerCode"/>
          </TriggerCode>
          <MarketingCommsConsent>
            <xsl:value-of select="Marketingcommsconsent"/>
          </MarketingCommsConsent>
          <BulkEmailConsent>
            <xsl:value-of select="EmailOptIn"/>
          </BulkEmailConsent>
          <PhoneConsent>
            <xsl:value-of select="PhoneOptIn"/>
          </PhoneConsent>
          <SMSConsent>
            <xsl:value-of select="SMSOptIn"/>
          </SMSConsent>
        </Echo>
      </xsl:for-each>
    </Data>
  </xsl:template>
</xsl:stylesheet>
