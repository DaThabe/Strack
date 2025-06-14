# 设计

```mermaid
erDiagram
    Activity ||--|| Source : have
    Activity ||--|| User : 
    Activity ||--|| Metrics : have
    Activity ||--o{ Record : have
    User ||--o{ Activity : have
    User ||--o| Certificate : have

    Activity {
        string Title
        DateTime Time
    }

    User {
        PlatformType Type
        long ExternalId
    }

    Certificate{
        CertificateType Type
        string Content
    }

    Source {
        PlatformType Type
        long ExternalId
    }

    Record {
        DateTime Time
        double Lat
        double Lon
    }
```