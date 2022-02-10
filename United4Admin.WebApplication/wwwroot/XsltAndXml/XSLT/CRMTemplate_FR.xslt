<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
    xmlns:msxsl="urn:schemas-microsoft-com:xslt" exclude-result-prefixes="msxsl">
  <xsl:output method="xml" indent="yes"/>
  <xsl:template match="/">
    <Data>
      <xsl:for-each select="//Echo">
        <Echo>
          <DateTime>
            <xsl:value-of select="DateOfRequest"/>
          </DateTime>
          <No>
            <xsl:value-of select="supporterID"/>
          </No>
          <PledgeType>
            <xsl:value-of select="PledgeType"/>
          </PledgeType>
          <ProductCode>
            <xsl:value-of select="ProductCode"/>
          </ProductCode>
          <Email>
            <xsl:value-of select="EmailAddress"/>
          </Email>
          <ChildProjectSequence>
            <xsl:value-of select="ChildID"/>
          </ChildProjectSequence>
          <ActionCode></ActionCode>
          <PurposeCode>
            <xsl:value-of select="DonationvariantID"/>
          </PurposeCode>
          <Salutation>
            <xsl:value-of select="Title"/>
          </Salutation>
          <FirstName>
            <xsl:value-of select="Forename"/>
          </FirstName>
          <SurName>
            <xsl:value-of select="Surname"/>
          </SurName>
          <CompanyName></CompanyName>
          <PhoneNo>
            <xsl:value-of select="PhoneNum1"/>
          </PhoneNo>
          <MobilePhoneNo>
            <xsl:value-of select="PhoneNum2"/>
          </MobilePhoneNo>
          <BirthDate>
            <xsl:value-of select="YearOfBirth"/>
          </BirthDate>
          <Address>
            <xsl:value-of select="StreetName"/>
          </Address>
          <Address2>
            <xsl:value-of select="AddressLine2"/>
          </Address2>
          <PostCode>
            <xsl:value-of select="Postcode"/>
          </PostCode>
          <City>
            <xsl:value-of select="AddressLine3"/>
          </City>
          <CountryCode>
            <xsl:value-of select="Country"/>
          </CountryCode>
          <PaymentMethodCode>
            <xsl:value-of select="PaymentMethod"/>
          </PaymentMethodCode>
          <IBAN>
            <xsl:value-of select="IBAN"/>
          </IBAN>
          <SWIFTCode></SWIFTCode>
          <Incidentcommentsub></Incidentcommentsub>
          <AmountPerPeriod>
            <xsl:value-of select="Donationamount"/>
          </AmountPerPeriod>
          <BillingPeriod>
            <xsl:value-of select="Donationfrequency"/>
          </BillingPeriod>
          <CatalogueID></CatalogueID>
          <CatalogueQuantity></CatalogueQuantity>
          <Month></Month>
          <DataProcessingConsent>
            <xsl:value-of select="Dataprocessingconsent"/>
          </DataProcessingConsent>
          <MarketingCommsConsent>
            <xsl:value-of select="Marketingcommsconsent"/>
          </MarketingCommsConsent>
          <EmailConsent>
            <xsl:value-of select="EmailOptIn"/>
          </EmailConsent>
          <PhoneConsent>
            <xsl:value-of select="PhoneOptIn"/>
          </PhoneConsent>
          <PostalConsent>
            <xsl:value-of select="DirectMailOptIn"/>
          </PostalConsent>
          <EmailSignUp>
            <xsl:value-of select="EmailSignUp"/>
          </EmailSignUp>
          <LeadSignUp>
            <xsl:value-of select="LeadsSignUp"/>
          </LeadSignUp>
          <ContentBasedEmailSignUp>
            <xsl:value-of select="ContentBasedLeadSignUp"/>
          </ContentBasedEmailSignUp>
        </Echo>
      </xsl:for-each>
    </Data>
  </xsl:template>
</xsl:stylesheet>
