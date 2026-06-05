# Message Estimator

MessageEstimator, yazım hatalarına toleranslı mesaj sınıflandırma yaklaşımını göstermek için hazırlanmış sade bir **ASP.NET Core Web API** projesidir.

Proje, gelen serbest metinleri önceden tanımlanmış kapsamlarla karşılaştırır ve sonucu `Exact`, `Typo` veya `Irrelevant` olarak döndürür. Yazım hatalarına tolerans göstermek için **Fastenshtein** kütüphanesi kullanılır.

## Özellikler

- Serbest metin mesaj analizi
- Türkçe karakter ve metin normalizasyonu
- Fastenshtein ile yazım hatası toleransı
- `Exact`, `Typo`, `Irrelevant` sonuç tipleri
- Tekil ve toplu mesaj analizi

## Makale

Bu proje, yazım hatalarına toleranslı mesaj sınıflandırma yaklaşımını anlattığım makale serisinin örnek uygulamasıdır.

- [Yazım Hatalarına Toleranslı Mesaj Sınıflandırma](https://mustafadikyar.medium.com/mesaj-sınıflandırma-serbest-metinlerde-doğru-kapsamı-bulmak-6fb6591c99b7)
- [Kavramdan Koda: Fastenshtein ile Yazım Hatalarına Toleranslı Mesaj Analizi](https://mustafadikyar.medium.com/kavramdan-koda-fastenshtein-kütüphanesi-ile-mesaj-sınıflandırma-96207d1358c4)
