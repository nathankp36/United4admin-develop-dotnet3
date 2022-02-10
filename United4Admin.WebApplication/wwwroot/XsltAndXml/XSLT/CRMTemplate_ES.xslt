<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
    xmlns:msxsl="urn:schemas-microsoft-com:xslt" exclude-result-prefixes="msxsl">
  <xsl:output method="xml" indent="yes"/>
  <xsl:template match="/">
    <Data>
      <xsl:for-each select="//Echo">
        <Echo>
          <Date>
            <xsl:value-of select="DateOfRequest"/>
          </Date>
          <AdrStatusID>
            <xsl:value-of select="AdrStatusID"/>
          </AdrStatusID>
          <AdrTypeID>
            <xsl:value-of select="AdrTypeID"/>
          </AdrTypeID>
          <OrgName2Code1Descr>
            <xsl:value-of select="OrgName2Code1Descr"/>
          </OrgName2Code1Descr>
          <OrgName2Code2Descr>
            <xsl:value-of select="OrgName2Code2Descr"/>
          </OrgName2Code2Descr>
          <GivenName>
            <xsl:value-of select="Forename"/>
          </GivenName>
          <MiddleName>
            <xsl:value-of select="MiddleName"/>
          </MiddleName>
          <FamilyName>
            <xsl:value-of select="Surname"/>
          </FamilyName>
          <MaritalStatusDescr>
            <xsl:value-of select="MaritalStatusDescr"/>
          </MaritalStatusDescr>
          <GenderCode>
            <xsl:value-of select="gender"/>
          </GenderCode>
          <BirthDate>
            <xsl:value-of select="YearOfBirth"/>
          </BirthDate>
          <AddDate>
            <xsl:value-of select="AddDate"/>
          </AddDate>
          <FamilyName2>
            <xsl:value-of select="FamilyName2"/>
          </FamilyName2>
          <OrgName1>
            <xsl:value-of select="OrgName1"/>
          </OrgName1>
          <OrgName2>
            <xsl:value-of select="OrgName2"/>
          </OrgName2>
          <OrgDepartmentName>
            <xsl:value-of select="OrgDepartmentName"/>
          </OrgDepartmentName>
          <OrgRoleDescr>
            <xsl:value-of select="OrgRoleDescr"/>
          </OrgRoleDescr>
          <PartnershipName>
            <xsl:value-of select="PartnershipName"/>
          </PartnershipName>
          <Salutation>
            <xsl:value-of select="Title"/>
          </Salutation>
          <HouseNumber>
            <xsl:value-of select="HouseNumber"/>
          </HouseNumber>
          <Street1>
            <xsl:value-of select="StreetName"/>
          </Street1>
          <Street2>
            <xsl:value-of select="AddressLine2"/>
          </Street2>
          <Street3>
            <xsl:value-of select="Street3"/>
          </Street3>
          <City>
            <xsl:value-of select="AddressLine3"/>
          </City>
          <StatesProvCode>
            <xsl:value-of select="StatesProvCode"/>
          </StatesProvCode>
          <CountryCode>
            <xsl:value-of select="Country"/>
          </CountryCode>
          <RegionDescr>
            <xsl:value-of select="RegionDescr"/>
          </RegionDescr>
          <PostalCode>
            <xsl:value-of select="Postcode"/>
          </PostalCode>
          <NoMail>
            <xsl:value-of select="NoMail"/>
          </NoMail>
          <NoMailID>
            <xsl:value-of select="NoMailID"/>
          </NoMailID>
          <EmailAddress>
            <xsl:value-of select="EmailAddress"/>
          </EmailAddress>
          <SpokenLanguageCode>
            <xsl:value-of select="SpokenLanguageCode"/>
          </SpokenLanguageCode>
          <PrintedLanguageCode>
            <xsl:value-of select="PrintedLanguageCode"/>
          </PrintedLanguageCode>
          <ReceiptingID>
            <xsl:value-of select="ReceiptingID"/>
          </ReceiptingID>
          <MotivationID>
            <xsl:value-of select="MotivationID"/>
          </MotivationID>
          <ReferenceText>
            <xsl:value-of select="ReferenceText"/>
          </ReferenceText>
          <PhoneTypeID1>
            <xsl:value-of select="PhoneTypeID1"/>
          </PhoneTypeID1>
          <PhoneNumber1>
            <xsl:value-of select="PhoneNum1"/>
          </PhoneNumber1>
          <PhoneTypeID2>
            <xsl:value-of select="PhoneTypeID2"/>
          </PhoneTypeID2>
          <PhoneNumber2>
            <xsl:value-of select="PhoneNum2"/>
          </PhoneNumber2>
          <TransactionID>
            <xsl:value-of select="PaymentTransactionID"/>
          </TransactionID>
          <ContactID>
            <xsl:value-of select="supporterID"/>
          </ContactID>
          <TaxId>
            <xsl:value-of select="taxId"/>
          </TaxId>
          <DataProcessingConsent>
            <xsl:value-of select="Dataprocessingconsent"/>
          </DataProcessingConsent>
          <MarketingCommsConsent>
            <xsl:value-of select="Marketingcommsconsent"/>
          </MarketingCommsConsent>
          <PaymentMethod>
            <xsl:value-of select="PaymentMethod"/>
          </PaymentMethod>
          <ExternalPaymentToken>
            <xsl:value-of select="ExternalPaymentToken"/>
          </ExternalPaymentToken>
          <IBAN>
            <xsl:value-of select="IBAN"/>
          </IBAN>
          <ProductID>
            <xsl:value-of select="ProductID"/>
          </ProductID>
          <Donationamount>
            <xsl:value-of select="Donationamount"/>
          </Donationamount>
          <DonationvariantID>
            <xsl:value-of select="DonationvariantID"/>
          </DonationvariantID>
          <Donationfrequency>
            <xsl:value-of select="Donationfrequency"/>
          </Donationfrequency>
          <ChildID>
            <xsl:value-of select="ChildID"/>
          </ChildID>
        </Echo>
      </xsl:for-each>
    </Data>
  </xsl:template>
</xsl:stylesheet>
