<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
    xmlns:msxsl="urn:schemas-microsoft-com:xslt" exclude-result-prefixes="msxsl">
  <xsl:output method="xml" indent="yes"/>
  <xsl:template match="/">
    <Data>
      <xsl:for-each select="//Echo">
        <Echo>
          <RequestType>
            <xsl:value-of select="RequestType"/>
          </RequestType>
          <TransactionReferenceNo>
            <xsl:value-of select="TransactionReferenceNo"/>
          </TransactionReferenceNo>
          <RequestReferenceNo>
            <xsl:value-of select="RequestReferenceNo"/>
          </RequestReferenceNo>
          <DateOfRequest>
            <xsl:value-of select="DateOfRequest"/>
          </DateOfRequest>
          <TriggerCode>
            <xsl:value-of select="TriggerCode"/>
          </TriggerCode>
          <ConsentStatementID>
            <xsl:value-of select="UKConsentStatementID"/>
          </ConsentStatementID>
          <supporterID>
			  <xsl:value-of select="WvSupporterId"/>
          </supporterID>
          <SupporterType>
            <xsl:value-of select="SupporterType"/>
          </SupporterType>
          <Title>
            <xsl:value-of select="Title"/>
          </Title>
          <Forename>
            <xsl:value-of select="Forename"/>
          </Forename>
          <Surname>
            <xsl:value-of select="Surname"/>
          </Surname>
          <Organisation_GroupName>
            <xsl:value-of select="Organisation_GroupName"/>
          </Organisation_GroupName>
          <Organisation_GroupType>
            <xsl:value-of select="Organisation_GroupType"/>
          </Organisation_GroupType>
          <gender>
            <xsl:value-of select="gender"/>
          </gender>
          <YearOfBirth>
            <xsl:value-of select="YearOfBirth"/>
          </YearOfBirth>
          <GiftAid>
            <xsl:value-of select="TaxConsentOptIn"/>
          </GiftAid>
          <AddressStructure>
            <xsl:value-of select="AddressStructure"/>
          </AddressStructure>
          <buildingNumber>
            <xsl:value-of select="buildingNumber"/>
          </buildingNumber>
          <buildingName>
            <xsl:value-of select="buildingName"/>
          </buildingName>
          <StreetName>
            <xsl:value-of select="StreetName"/>
          </StreetName>
          <AddressLine2>
            <xsl:value-of select="AddressLine2"/>
          </AddressLine2>
          <AddressLine3>
            <xsl:value-of select="AddressLine3"/>
          </AddressLine3>
          <AddressLine4>
            <xsl:value-of select="AddressLine4"/>
          </AddressLine4>
          <Postcode>
            <xsl:value-of select="Postcode"/>
          </Postcode>
          <Country>
            <xsl:value-of select="Country"/>
          </Country>
          <PhoneNum1>
            <xsl:value-of select="PhoneNum1"/>
          </PhoneNum1>
          <PhoneNum2>
            <xsl:value-of select="PhoneNum2"/>
          </PhoneNum2>
          <EmailAddress>
            <xsl:value-of select="EmailAddress"/>
          </EmailAddress>
          <Location>
            <xsl:value-of select="Location"/>
          </Location>
          <CommitmentAmount>
            <xsl:value-of select="CommitmentAmount"/>
          </CommitmentAmount>
          <TransactionAmount>
            <xsl:value-of select="TransactionAmount"/>
          </TransactionAmount>
          <PaymentMethod>
            <xsl:value-of select="PaymentMethod"/>
          </PaymentMethod>
          <PaymentType>
            <xsl:value-of select="PaymentType"/>
          </PaymentType>
          <PaymentReference>
            <xsl:value-of select="PaymentReference"/>
          </PaymentReference>
          <PaymentFrequency>
            <xsl:value-of select="PaymentFrequency"/>
          </PaymentFrequency>
          <PreferredDDDate>
            <xsl:value-of select="PreferredDDDate"/>
          </PreferredDDDate>
          <SortCode>
            <xsl:value-of select="SortCode"/>
          </SortCode>
          <AccountCode>
            <xsl:value-of select="AccountCode"/>
          </AccountCode>
          <CardHolder>
            <xsl:value-of select="CardHolder"/>
          </CardHolder>
          <ChildID>
            <xsl:value-of select="ChildID"/>
          </ChildID>
          <PreferredContinent>
            <xsl:value-of select="PreferredContinent"/>
          </PreferredContinent>
          <PreferredCountry>
            <xsl:value-of select="PreferredCountry"/>
          </PreferredCountry>
          <PreferredGender>
            <xsl:value-of select="PreferredGender"/>
          </PreferredGender>
          <PreferredAge>
            <xsl:value-of select="PreferredAge"/>
          </PreferredAge>
          <Receipt_Required>
            <xsl:value-of select="Receipt_Required"/>
          </Receipt_Required>
          <Fundraised>
            <xsl:value-of select="Fundraised"/>
          </Fundraised>
          <CommitmentCode>
            <xsl:value-of select="CommitmentCode"/>
          </CommitmentCode>
          <PaymentTransactionID>
            <xsl:value-of select="PaymentTransactionID"/>
          </PaymentTransactionID>
          <Comments>
            <xsl:value-of select="Comments"/>
          </Comments>
          <ResponseEntity>
            <xsl:value-of select="ResponseEntity"/>
          </ResponseEntity>
          <ScheduledPayID>
            <xsl:value-of select="ScheduledPayID"/>
          </ScheduledPayID>
          <AiName1>
            <xsl:value-of select="AiName1"/>
          </AiName1>
          <AiValue1>
            <xsl:value-of select="AiValue1"/>
          </AiValue1>
          <AiName2>
            <xsl:value-of select="AiName2"/>
          </AiName2>
           <AiValue2>
            <xsl:value-of select="AiValue2"/>
          </AiValue2>
          <FaithAndFamily>
            <xsl:value-of select="FaithAndFamily"/>
          </FaithAndFamily>
          <DirectMailOptIn>
            <xsl:value-of select="DirectMailOptIn"/>
          </DirectMailOptIn>
          <EmailOptIn>
            <xsl:value-of select="EmailOptIn"/>
          </EmailOptIn>
          <PhoneOptIn>
            <xsl:value-of select="PhoneOptIn"/>
          </PhoneOptIn>
          <SMSOptIn>
            <xsl:value-of select="SMSOptIn"/>
          </SMSOptIn>
          <DirectMailOptOut>
            <xsl:value-of select="DirectMailOptOut"/>
          </DirectMailOptOut>
          <EmailOptOut>
            <xsl:value-of select="EmailOptOut"/>
          </EmailOptOut>
          <PhoneOptOut>
            <xsl:value-of select="PhoneOptOut"/>
          </PhoneOptOut>
          <SMSOptOut>
            <xsl:value-of select="SMSOptOut"/>
          </SMSOptOut>      
        </Echo>
      </xsl:for-each>
    </Data>
  </xsl:template>
</xsl:stylesheet>
