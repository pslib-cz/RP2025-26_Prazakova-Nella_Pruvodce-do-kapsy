import { useEffect, useRef, useState } from 'react';
import { Icon } from '@iconify/react';

import type { Point } from '../../Types/MapType';

import {
  getPointDescription,
  getPointFieldLabel,
  getPointIconFromPoint,
  getPointRoomLabel,
  getPointSpecialization,
  getPointThemeClass,
  getPointTitle,
  getPointAreStudents,
} from '../../pointIconUtils';

import style from '../../Styles/PointDetailPanel.module.css';

interface PointDetailPanelProps {
  point: Point | null;
  isDesktop: boolean;
  onClose: () => void;
}

const ANIMATION_MS = 260;

const PointDetailPanel: React.FC<PointDetailPanelProps> = ({
  point,
  isDesktop,
  onClose,
}) => {
  const [renderedPoint, setRenderedPoint] = useState<Point | null>(point);
  const [isClosing, setIsClosing] = useState(false);
  const [expanded, setExpanded] = useState(false);
  const touchStartY = useRef<number | null>(null);

  useEffect(() => {
    if (point) {
      setRenderedPoint(point);
      setIsClosing(false);
      setExpanded(false);
      return;
    }

    if (renderedPoint) {
      setIsClosing(true);

      const timeout = window.setTimeout(() => {
        setRenderedPoint(null);
        setIsClosing(false);
        setExpanded(false);
      }, ANIMATION_MS);

      return () => window.clearTimeout(timeout);
    }
  }, [point, renderedPoint]);

  useEffect(() => {
    setExpanded(false);
  }, [renderedPoint?.pointId]);

  if (!renderedPoint) return null;

  const title = getPointTitle(renderedPoint);
  const room = getPointRoomLabel(renderedPoint);
  const description = getPointDescription(renderedPoint);
  const fieldLabel = getPointFieldLabel(renderedPoint);
  const iconName = getPointIconFromPoint(renderedPoint);
  const themeClass = getPointThemeClass(renderedPoint);
  const specialization = getPointSpecialization(renderedPoint);
  const areStudents = getPointAreStudents(renderedPoint);

  const rawPoint = renderedPoint as Point & {
    teachers?: {
      teacherId?: string;
      firstN?: string;
      lastN?: string;
      degree?: string | null;
    }[];
    subjects?: {
      subjectId?: string;
      name?: string;
      acronym?: string | null;
      description?: string | null;
    }[];
    note?: {
      text?: string | null;
      studentName?: string | null;
      studentField?: string | null;
    } | null;
  };

  const teachers = rawPoint.teachers ?? [];
  const subjects = rawPoint.subjects ?? [];
  const note = rawPoint.note ?? null;

  const teacherNames = teachers
    .map(teacher =>
      `${teacher.degree ? `${teacher.degree} ` : ''}${teacher.firstN ?? ''} ${
        teacher.lastN ?? ''
      }`.trim()
    )
    .filter(Boolean)
    .join(', ');

  return (
    <aside
      className={`${style.detailPanel} ${style[themeClass]} ${
        isDesktop ? style.desktopPanel : style.mobilePanel
      } ${expanded ? style.mobileExpanded : ''} ${
        isClosing ? style.isClosing : style.isOpen
      }`}
      onClick={event => event.stopPropagation()}
      onTouchStart={event => {
        touchStartY.current = event.touches[0].clientY;
      }}
      onTouchEnd={event => {
        if (touchStartY.current == null) return;

        const endY = event.changedTouches[0].clientY;
        const delta = touchStartY.current - endY;

        if (delta > 40) setExpanded(true);
        if (delta < -40 && expanded) setExpanded(false);

        touchStartY.current = null;
      }}
    >
      {!isDesktop && <div className={style.dragHandle} />}

      <button
        type="button"
        className={style.closeButton}
        onClick={onClose}
        aria-label="Zavřít detail stanoviště"
      >
        <Icon icon={isDesktop ? 'lucide:chevron-left' : 'lucide:x'} />
      </button>

      <div className={style.content}>
        <header className={style.header}>
          <div className={style.headerStart}>
            <div className={`${style.iconBox} ${style[themeClass]}`}>
              <Icon icon={iconName} />
            </div>

            <div className={style.headerText}>
              <h2>{title}</h2>
              <p>{room}</p>
            </div>
          </div>

          <span className={`${style.specializationBadge} ${style[themeClass]}`}>{specialization?.type}</span>
          
        </header>

        {!isDesktop && !expanded && (
          <button
            type="button"
            className={style.expandHint}
            onClick={() => setExpanded(true)}
          >
            <span>Zobrazit více</span>
            <Icon icon="lucide:chevron-up" />
          </button>
        )}

        <div className={style.fullContent}>
          
          <section className={style.section}>
            <span className={style.fieldLabel}>
            {fieldLabel}
          </span>
            <h3>
              <Icon icon="lucide:eye" />
              Co se dozvíte
            </h3>

            <p>{description || 'Popis tohoto stanoviště zatím nebyl vyplněn.'}</p>
            {areStudents && (
              <p>
                <strong>Na stanovišti budou přítomni studenti.</strong> Přijďte se na cokoli zeptat!
              </p>
            )}
          </section>

          {(teachers.length > 0 || note?.text) && (
            <section className={style.teacherCard}>
              <div className={style.teacherTop}>
                <div className={style.avatar} />

                <div>
                  <strong>{teacherNames || 'Vyučující'}</strong>
                  <span>Vyučující</span>
                </div>

                <Icon className={style.quoteIcon} icon="lucide:quote" />
              </div>

              {note?.text && (
                <div className={style.teacherNote}>
                  <span>Poznámka od studenta</span>
                  <p>{note.text}</p>

                  <small>
                    ~ {note.studentName || 'Student'}
                    {note.studentField ? `, ${note.studentField}` : ''}
                  </small>
                </div>
              )}
            </section>
          )}

          <section className={style.section}>
            <h3>
              <Icon icon="lucide:menu" />
              O oboru "{specialization?.type}"
            </h3>

            <p>{specialization?.description || 'Popis oboru zatím nebyl vyplněn.'}</p>
          </section>

          {subjects.length > 0 && (
            <section className={style.section}>
              <h3>
                <Icon icon="lucide:book-open" />
                Předměty
              </h3>

              <div className={style.subjectList}>
                {subjects.map(subject => (
                  <article
                    key={subject.subjectId ?? subject.name}
                    className={style.subjectCard}
                  >
                    <div className={style.subjectHeader}>
                      <strong>{subject.name}</strong>

                      {subject.acronym && <small>{subject.acronym}</small>}
                    </div>

                    <p>
                      {subject.note || 'Pro více informací se zeptejte na stanovišti.'}
                    </p>
                  </article>
                ))}
              </div>
            </section>
          )}
        </div>
      </div>
    </aside>
  );
};

export default PointDetailPanel;