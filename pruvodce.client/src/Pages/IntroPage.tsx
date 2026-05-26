import React, { useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import { useStaticData } from '../Hooks/useStaticData';
import style from '../Styles/IntroPage.module.css';

const IntroPage: React.FC = () => {
  const navigate = useNavigate();
  const { buildings, loading, error } = useStaticData();

  useEffect(() => {
    if (loading || error) return;

    const savedBuilding = localStorage.getItem('preferredBuilding');
    if (!savedBuilding) return;

    const savedBuildingId = Number(savedBuilding);
    const exists = buildings.some(building => building.buildingId === savedBuildingId);

    if (exists) {
      navigate(`/map/${savedBuildingId}`, { replace: true });
    } else {
      localStorage.removeItem('preferredBuilding');
    }
  }, [buildings, loading, error, navigate]);

  const handleSelectBuilding = (id: number) => {
    localStorage.setItem('preferredBuilding', id.toString());
    navigate(`/map/${id}`);
  };

  if (loading) {
    return (
      <div className={style.introContainer}>
        <div className={style.statusBox}>Načítání...</div>
      </div>
    );
  }

  if (error) {
    return (
      <div className={style.introContainer}>
        <div className={style.statusBox}>{error}</div>
      </div>
    );
  }

  return (
    <main className={style.introContainer}>
      <section className={style.card}>
        <header className={style.header}>
          <a href="https://www.prumyslovkaliberec.cz/" target="_blank" rel="noopener noreferrer">
            <img src="/logo.png" alt="Průmyslovka Liberec" className={style.logo} />
          </a>
        </header>

        <section className={style.selectorSection}>
          <div className={style.introText}>
          <h1 className={style.title}>Vyberte si areál</h1>
          <p>Zvolte budovu, ve které chcete začít. Areál můžete kdykoli změnit.</p>

</div>
<div className={style.introContent}>
          <div className={style.buttonGroup}>
            {buildings.map((building, index) => (
              <button
                key={building.buildingId}
                type="button"
                className={`${style.buildingButton} ${
                  index % 2 === 0 ? style.masarykovaButton : style.tyrsovaButton
                }`}
                onClick={() => handleSelectBuilding(building.buildingId)}
              >
                BUDOVA {building.name.toUpperCase()}
              </button>
            ))}
          </div>

          <p className={style.quizText}>
            Nevíte kam vyrazit? <button type="button" className={style.quizLink}>Vyplňte náš kvíz.</button>
          </p></div>
        </section>
      </section>
    </main>
  );
};

export default IntroPage;