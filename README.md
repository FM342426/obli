/develop
Ramas principales de Git Flow:

🔹 1. feature/*

Se crea desde develop.

Cuando terminas una feature, abrir un PR hacia develop.

🔹 2. release/*

Se crea desde develop cuando preparas una versión estable.

Abrir un PR hacia main (cuando liberas) y otro PR hacia develop (para mantener sincronía).

🔹 3. hotfix/*

Se crea desde main para corregir bugs en producción.

Al terminar, se suele abrir un PR hacia main y otro PR hacia develop.

✅ Resumiendo:

PR en features → hacia develop (nuevo código).

PR en releases → hacia main y develop (preparación de versión).

PR en hotfix → hacia main y develop (corrección urgente).
