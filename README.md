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

## שלילת היכולת של הספינה לירות למשך 5 שניות

במערכת זו, הספינה מאבדת את היכולת לירות למשך 5 שניות כתוצאה מאירוע מסוים (איסוף פריט). הלוגיקה מתבצעת על ידי ניהול משתנה שמונע מהשחקן לירות במהלך זמן זה.


### לוגיקת הסקריפט
- **ביטול זמני של ירי**: אם נמצאה קומפוננטת `LaserShooter`, היכולת לירות מושבתת למשך הזמן שהוגדר (ברירת מחדל: 5 שניות).
- **הרצת Coroutine**: הסקריפט משתמש ב-Coroutine כדי להשהות את היכולת לירות למשך הזמן המוגדר, ולאחר מכן מחזיר את היכולת.
- **השמדה עצמית**: לאחר שהטריגר מופעל, האובייקט המכיל את `ShootDisableTrigger` מושמד.

### קוד:
```csharp
using System.Collections;
using UnityEngine;

public class ShootDisableTrigger : MonoBehaviour
{
    [Tooltip("How many seconds to disable shooting for.")]
    [SerializeField] private float disableShootingDuration = 5f;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log($"ShootDisableTrigger triggered by {other.name}");

            LaserShooter laserShooter = other.GetComponent<LaserShooter>();
            if (laserShooter != null)
            {
                // Use a persistent CoroutineRunner to handle the coroutine
                CoroutineRunner.Instance.StartCoroutine(DisableShootingForDuration(laserShooter));
            }
            else
            {
                Debug.LogWarning($"LaserShooter component not found on {other.name}");
            }

            // Destroy the icon immediately after the collision
            Destroy(gameObject);
        }
        else
        {
            Debug.Log($"ShootDisableTrigger triggered by non-player object: {other.name}");
        }
    }

    private IEnumerator DisableShootingForDuration(LaserShooter laserShooter)
    {
        laserShooter.SetShootingEnabled(false);
        Debug.Log($"Shooting disabled for {laserShooter.name} for {disableShootingDuration} seconds!");

        yield return new WaitForSeconds(disableShootingDuration);

        laserShooter.SetShootingEnabled(true);
        Debug.Log($"Shooting re-enabled for {laserShooter.name} after {disableShootingDuration} seconds.");
    }
}
```





