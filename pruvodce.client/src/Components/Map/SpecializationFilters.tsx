import { useMemo } from 'react';
import { Icon } from '@iconify/react';
import type { Point } from '../../Types/MapType';
import { getFieldTypeLabel } from '../../pointIconUtils';
import style from '../../Styles/MapPage.module.css';

interface SpecializationFiltersProps {
  points: Point[];
  activeSpecializations: string[];
  onActiveSpecializationsChange: React.Dispatch<React.SetStateAction<string[]>>;
}

function getUniqueSpecializations(points: Point[]): Array<{
  id: string;
  name: string;
  type: number | string | null;
}> {
  const specMap = new Map<string, { id: string; name: string; type: number | string | null }>();

  for (const point of points) {
    if (!point.specialization) continue;

    const specId = point.specializationId ?? point.specialization.specializationId;
    if (!specId) continue;

    if (!specMap.has(specId)) {
      specMap.set(specId, {
        id: specId,
        name: point.specialization.name || 'Neznámý obor',
        type: point.specialization.type ?? point.specialization.type ?? null,
      });
    }
  }

  return Array.from(specMap.values());
}

const SpecializationFilters: React.FC<SpecializationFiltersProps> = ({
  points,
  activeSpecializations,
  onActiveSpecializationsChange,
}) => {
  const specializations = useMemo(() => {
    return getUniqueSpecializations(points);
  }, [points]);

  if (specializations.length === 0) {
    return null;
  }

  const toggleSpecialization = (specId: string) => {
    onActiveSpecializationsChange(previous => {
      if (previous.includes(specId)) {
        return previous.filter(id => id !== specId);
      }
      return [...previous, specId];
    });
  };

  return (
    <div className={style.specializationFilters}>
      <div className={style.specializationFiltersHeader}>
        <h2 className={style.panelLabel}>Obor</h2>
      </div>

      <div className={style.specializationList}>
        {specializations.map(spec => {
          const isActive = activeSpecializations.includes(spec.id);
          const fieldTypeLabel = getFieldTypeLabel(spec.type);

          return (
            <label
            key={spec.id}
            className={`${style.specializationItem} ${isActive ? style.specializationActive : style.specializationInactive}`}
            >
            <input
                type="checkbox"
                checked={isActive}
                onChange={() => toggleSpecialization(spec.id)}
            />

            <span className={style.fakeCheckbox}>
                {isActive && <Icon icon="lucide:check" width="14" height="14" />}
            </span>

            <span className={style.specializationText}>
                {fieldTypeLabel && fieldTypeLabel !== 'Bez oboru' && (
                <p>{fieldTypeLabel}</p>
                )}
            </span>
            </label>
          );
      })}
      </div>
    </div>
  );
};

export default SpecializationFilters;