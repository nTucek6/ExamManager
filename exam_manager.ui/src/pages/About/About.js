import "./About.css";

export default function About() {
  return (
    <div className="about-container">
      <h1>Examio – Sustav za prijavu ispita</h1>

      <section>
        <h2>Uvod</h2>
        <p>
          Examio je moderan informacijski sustav dizajniran za studente,
          profesore i administratore s ciljem digitalizacije i optimizacije
          prijava ispita na visokoškolskim ustanovama. Automatizacijom procesa
          značajno se smanjuje administrativno opterećenje i povećava dostupnost
          informacija.
        </p>
        <p>
          Sustav pruža intuitivno korisničko sučelje, brz pristup ključnim
          podacima i jednostavno upravljanje akademskim obavezama.
        </p>
      </section>

      <section>
        <h2>Funkcionalnosti</h2>

        <div className="role-section">
          <h3>🎓 Za studente</h3>
          <ul>
            <li>✅ Registracija i prijava putem e-maila i lozinke</li>
            <li>✅ Prijava i odjava ispita unutar zadanih rokova</li>
            <li>✅ Tablični i kalendarski prikaz prijavljenih ispita</li>
          </ul>
        </div>

        <div className="role-section">
          <h3>👨‍🏫 Za profesore</h3>
          <ul>
            <li>✅ Kreiranje i brisanje termina ispita</li>
            <li>✅ Pregled prijava studenata po ispitima</li>
          </ul>
        </div>

        <div className="role-section">
          <h3>🛠️ Za administratore</h3>
          <ul>
            <li>✅ Upravljanje korisnicima i dodjela uloga</li>
          </ul>
        </div>
      </section>

      <section>
        <h2>Tehnologije</h2>
        <ul className="tech-list">
          <li>
            <strong>Frontend:</strong> React, JavaScript, HTML, CSS
          </li>
          <li>
            <strong>Backend:</strong> C#, .NET Core, ASP.NET
          </li>
          <li>
            <strong>Baza podataka:</strong> PostgreSQL
          </li>
          <li>
            <strong>Arhitektura:</strong> MVC (Model-View-Controller)
          </li>
        </ul>
      </section>
    </div>
  );
}
