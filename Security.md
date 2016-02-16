# Securing Crystal Quartz pages #

It is a common practice to restrict access to the Crystal Quartz Panel in the production mode. If you are using standard ASP.NET security mechanism then you can reach this by adding the following rows to the web.config file:

```
<location path="CrystalQuartzPanel.axd">  
    <system.web>  
        <authorization>  
            <deny users="?" />  
        </authorization>  
    </system.web>  
</location>  
```