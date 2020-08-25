library(readxl)
D1 <- read_excel("C:/BRM/LANDIS_II/GitCode/brmiranda/Extension-Root-Rot/Docs/Root rot extension calibration usnig Autralian data.xlsx", 
                                                                  sheet = "Combined")

fit1 <- lm(Pop ~ soilT * ph_m,D1[complete.cases(D1),])
summary(fit1)
AIC(fit1)
#plot(fit1)

fit2 <- lm(Pop ~ soilT,D1[complete.cases(D1),])
summary(fit2)
AIC(fit2)

fit3 <- lm(Pop ~ soilT + ph_m,D1[complete.cases(D1),])
summary(fit3)
AIC(fit3)

fit4 <- lm(Pop ~ poly(soilT,2) + poly(ph_m,2),D1[complete.cases(D1),])
summary(fit4)
AIC(fit4)

fit5 <- lm(Pop ~ poly(soilT,2) + ph_m,D1[complete.cases(D1),])
summary(fit5)
AIC(fit5)

fitIndex <- lm(Pop ~ wetindex,D1[complete.cases(D1),])
summary(fitIndex)
AIC(fitIndex)

fitIndex2 <- lm(Pop ~ Index2,D1[complete.cases(D1),])
summary(fitIndex2)
AIC(fitIndex2)


cor(D1$Pop,D1$wetindex)
cor.test(D1$Pop,D1$wetindex)

cor(D1$Pop, D1$Index2)
cor.test(D1$Pop,D1$Index2)

warmSoil <- D1[D1$soilT > 10,]
cor.test(warmSoil$Pop,warmSoil$wetindex)
cor.test(warmSoil$Pop,warmSoil$Index2)

fitIndexWarm <- lm(Pop ~ wetindex,warmSoil[complete.cases(warmSoil),])
summary(fitIndexWarm)
AIC(fitIndexWarm)

fitIndex2Warm <- lm(Pop ~ Index2,warmSoil[complete.cases(warmSoil),])
summary(fitIndex2Warm)
AIC(fitIndex2Warm)


plot(warmSoil$Index2,jitter(warmSoil$PopIndex,1),col=as.factor(warmSoil$FieldCapacity))


fitIndex2byFWC <- lm(PopIndex ~ Index2 + FreeWaterCapacity,warmSoil)
summary(fitIndex2byFWC)
AIC(fitIndex2byFWC)


fitIndex2byFWCInt <- lm(PopIndex ~ Index2 * FreeWaterCapacity,warmSoil)
summary(fitIndex2byFWCInt)
AIC(fitIndex2byFWCInt)

fitIndex2bySat <- lm(PopIndex ~ Index2 + Saturation,warmSoil)
summary(fitIndex2bySat)
AIC(fitIndex2bySat)

fitIndex2bySatInt <- lm(PopIndex ~ Index2 * Saturation,warmSoil)
summary(fitIndex2bySatInt)
AIC(fitIndex2bySatInt)

fitIndex2byFC <- lm(PopIndex ~ Index2 + FieldCapacity,warmSoil)
summary(fitIndex2byFC)
AIC(fitIndex2byFC)

fitIndex2byFCInt <- lm(PopIndex ~ Index2 * FieldCapacity,warmSoil)
summary(fitIndex2byFCInt)
AIC(fitIndex2byFCInt)

