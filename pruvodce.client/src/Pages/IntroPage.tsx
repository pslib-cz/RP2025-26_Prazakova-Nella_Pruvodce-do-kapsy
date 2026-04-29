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

  if (loading) return <div>Načítání...</div>;
  if (error) return <div>{error}</div>;

  return (
    <div className={style.introContainer}>
      <div className={style.contentOverlay}>
        <div className={style.header}>
          <img src="/prumLogo.png" alt="Logo" className={style.logo} />
          <h1 className={style.mainTitle}>DEN OTEVŘENÝCH DVEŘÍ</h1>
          <h2 className={style.subTitle}>VYBERTE SI AREÁL</h2>
        </div>

        <div className={style.footer}>
          {buildings.map(building => (
            <button
              key={building.buildingId}
              className={style.quizButton}
              onClick={() => handleSelectBuilding(building.buildingId)}
            >
              {building.name}
            </button>
          ))}
        </div>
      </div>
    </div>
  );
};

export default IntroPage;