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
            <img src={`${import.meta.env.BASE_URL}logo.png`} alt="Průmyslovka Liberec" className={style.logo} />
          </a>
        </header>

        <section className={style.selectorSection}>

          <div className={style.introText}>
            <div className={style.iconWrapper}>
              <svg xmlns="http://www.w3.org/2000/svg" width="1em" height="1em" viewBox="0 0 24 24">
                <path d="M0 0h24v24H0z" fill="none" />
                <path fill="none" stroke="#3f4bf7" strokeLinecap="round" strokeLinejoin="round" strokeWidth="2" d="m3 7l6-3l6 3l6-3v13l-6 3l-6-3l-6 3zm6-3v13m6-10v13" />
              </svg>

            <p className={style.subtitle}>
              Průvodce do kapsy</p>
              </div>
            <h1 className={style.title}>Vyberte si <br/><span className={style.blueText}>areál</span></h1>
            <p>Zvolte budovu, ve které chcete začít. <br/>
                Areál můžete kdykoli změnit.</p>
          </div>

          <div className={style.introContent}>
          <div className={style.buttonGroup}>
            {buildings.map((building) => (
              <button
                key={building.buildingId}
                type="button"
                className={style.buildingButton}
                onClick={() => handleSelectBuilding(building.buildingId)}
              >
                <div className={style.buildingButtonContent}>
                  <h2 className={style.buildingTitle}>
                    Budova {building.name}
                  </h2>

                  <div className={style.buildingAddress}>
                    <svg
                      className={style.locationIcon}
                      viewBox="0 0 24 24"
                      fill="none"
                      stroke="currentColor"
                      strokeWidth="2"
                    >
                      <path d="M12 21s-6-5.33-6-11a6 6 0 1 1 12 0c0 5.67-6 11-6 11Z" />
                      <circle cx="12" cy="10" r="2.5" />
                    </svg>

                    <span>{building.address}</span>
                  </div>
                </div>

                <div className={style.buildingArrow}>
                  <svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 12 24" className={style.arrowIcon}>
                    <path d="M0 0h12v24H0z" fill="none" />
                    <defs>
                      <path id="SVG1pzpbdYY" fill="#ffffff" d="m7.588 12.43l-1.061 1.06L.748 7.713a.996.996 0 0 1 0-1.413L6.527.52l1.06 1.06l-5.424 5.425z" />
                    </defs>
                    <use fillRule="evenodd" href="#SVG1pzpbdYY" transform="rotate(-180 5.02 9.505)" />
                  </svg>
                </div>
              </button>
            ))}
          </div>
        </div>
        </section>
      </section>
    </main>
  );
};

export default IntroPage;