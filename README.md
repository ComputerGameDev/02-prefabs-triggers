# שיפור המשחק

מטלה זו כוללת שלושה חלקים עיקריים:

- שינוי גבולות המשחק כך שהעולם יתפקד כעולם עגול בציר האופקי ועולם סגור בציר האנכי.
- הוספת מערכת נקודות בריאות לחללית של השחקן, כולל הורדת נקודות בריאות בעת פגיעה, הוספת נקודות בריאות באיסוף פריטים, ותצוגה גרפית של מצב הבריאות.
- שלילת היכולת מהשחקן לירות למשך 5 שניות .  

---
## שינוי גבולות המשחק

### עולם עגול בציר האופקי
כאשר האובייקט יוצא מהגבול הימני של המסך, הוא מופיע בצד השמאלי, ולהפך. 

#### קוד:
```csharp
if (transform.position.x < screenLeft)
{
    transform.position = new Vector3(screenRight, transform.position.y, transform.position.z);
}
else if (transform.position.x > screenRight)
{
    transform.position = new Vector3(screenLeft, transform.position.y, transform.position.z);
}
```
---

### עולם עגול בציר האנכי
מניעה מהשחקן לצאת מהמסך בציר האנכי באמצעות הגבלת הערכים של ציר ה-Y.

#### קוד:
```csharp
float newY = Mathf.Clamp(transform.position.y + movementVector.y, screenBottom, screenTop);
transform.position = new Vector3(transform.position.x + movementVector.x, newY, transform.position.z);
```
---
## מימוש מערכת הבריאות

### תצוגת בריאות באמצעות סמלי לבבות
הלבבות מוצגים על המסך באמצעות **Canvas** ומנוהלים על ידי סקריפט ה-`Health`. הסקריפט כולל:
- רשימה של **תמונות לבבות** (`Image`) המייצגות את מצב הבריאות.
- סמלים שונים ללבבות מלאים וריקים.

#### לוגיקת הסקריפט:
1. בכל סצנה, שלושת הלבבות הלבבות מתעדכנים בהתאם למספר נקודות הבריאות.
2. לבבות מלאים מיוצגים על ידי ה- `fullHeart`, ולבבות ריקים על ידי `emptyHeart`.

#### קוד:
```csharp
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    [SerializeField]
    public static int health = 3;
    [SerializeField]
    private Image[] hearts;
    [SerializeField]
    private Sprite fullHeart;
    [SerializeField]
    private Sprite emptyHeart;

    void Update()
    {
        // איפוס כל הלבבות למצב ריק
        foreach (Image img in hearts)
        {
            img.sprite = emptyHeart;
        }
        // עדכון הלבבות בהתאם לנקודות הבריאות
        for (int i = 0; i < health; i++)
        {
            hearts[i].sprite = fullHeart;
        }
    }
}
```

---

### פגיעה והורדת בריאות
סקריפט **`HealthWhenDestroy`** מטפל בהתנגשות עם אויבים ומפחית את מספר נקודות הבריאות. כאשר הבריאות יורדת מתחת ל-0, השחקן (והאובייקט שנפגע) מושמד.

#### לוגיקת הסקריפט:
1. בעת התנגשות עם אובייקט בעל תג תואם (`triggeringTag`), הבריאות מופחתת.
2. אם הבריאות יורדת מתחת ל-0, גם השחקן וגם האובייקט שהשפיע עליו נהרסים.

#### קוד:
```csharp
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthWhenDestroy : MonoBehaviour {
    [Tooltip("Every object tagged with this tag will trigger the destruction of this object")]
    [SerializeField] string triggeringTag;

   private void OnTriggerEnter2D(Collider2D other) {
        // בדיקה אם האובייקט מתנגש בתג המתאים
        if (other.tag == triggeringTag && enabled) {
            Health.health--; // הפחתת הבריאות

            // אם הבריאות נגמרה, השמדת השחקן והאובייקט הפוגע
            if (Health.health < 0) {
                Destroy(this.gameObject);
                Destroy(other.gameObject);
            }
        }
    }
}
```

---

### הקשרים בין הסקריפטים
- **`Health`**:
  - מנהל את מצב הבריאות ומציג אותו באמצעות סמלי לבבות.
- **`HealthWhenDestroy`**:
  - מפחית בריאות בעקבות פגיעות ומתקשר ישירות למשתנה הסטטי `Health.health` כדי לעדכן את המצב.

--.

---





