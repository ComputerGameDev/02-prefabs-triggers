# שיפור המשחק

מטלה זו כוללת שלושה חלקים עיקריים:

- שינוי גבולות המשחק כך שהעולם יתפקד כעולם עגול בציר האופקי ועולם סגור בציר האנכי.
- הוספת מערכת נקודות בריאות לחללית של השחקן, כולל הורדת נקודות בריאות בעת פגיעה, הוספת נקודות בריאות באיסוף פריטים, ותצוגה גרפית של מצב הבריאות.
- מניעת היכולת מהשחקן לירות למשך 5 שניות .  

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
```csharp



