import { Icon } from '@iconify/react';
import style from '../../Styles/MapPage.module.css';

type PointIcon = 'Talk' | 'Hand' | 'Ucebna' | 'Jine';

interface PointTypeFiltersProps {
  activeTypes: PointIcon[];
  onChange: React.Dispatch<React.SetStateAction<PointIcon[]>>;
}

const options: {
  value: PointIcon;
  title: string;
  description: string;
  icon: string;
  className: string;
}[] = [
  {
    value: 'Talk',
    title: 'Přednáška',
    description: 'Sedněte si a poslouchejte',
    icon: 'lucide:presentation',
    className: 'pointTalk',
  },
  {
    value: 'Hand',
    title: 'Praktické stanoviště',
    description: 'Vyzkoušejte si něco z oboru',
    icon: 'lucide:hand',
    className: 'pointHand',
  },
  {
    value: 'Ucebna',
    title: 'Učebna',
    description: 'Nahlédněte do třídy a promluvte si s učiteli',
    icon: 'lucide:door-open',
    className: 'pointUcebna',
  },
  {
    value: 'Jine',
    title: 'Ostatní',
    description: '',
    icon: 'lucide:ellipsis',
    className: 'pointJine',
  },
];

const PointTypeFilters: React.FC<PointTypeFiltersProps> = ({
  activeTypes,
  onChange,
}) => {
  const toggleType = (type: PointIcon) => {
    onChange(previous => {
      if (previous.includes(type)) {
        return previous.filter(item => item !== type);
      }

      return [...previous, type];
    });
  };

  return (
    <div className={style.typeList}>
      {options.map(option => {
        const checked = activeTypes.includes(option.value);

        return (
          <label key={option.value} className={style.typeItem}>
            <input
              type="checkbox"
              checked={checked}
              onChange={() => toggleType(option.value)}
            />

            <span className={style.fakeCheckbox}>
              {checked && <Icon icon="lucide:check" width="14" height="14" />}
            </span>

            <span className={`${style.typeIcon} ${style[option.className]}`}>
              <Icon icon={option.icon} width="20" height="20" />
            </span>

            <span className={style.typeText}>
              <strong>{option.title}</strong>
              {option.description && <small>{option.description}</small>}
            </span>
          </label>
        );
      })}
    </div>
  );
};

export default PointTypeFilters;