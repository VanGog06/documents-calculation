import React, { useMemo } from 'react';

import { CircularProgress, List, ListItemText } from '@material-ui/core';

import { DataState } from '../../models/DataState';
import styles from './CalculationResult.module.scss';
import { ICalculationResultProps } from './ICalculationResultProps';

export const CalculationResult: React.FC<ICalculationResultProps> = ({
  calculationResult,
  outputCurrency,
}: ICalculationResultProps): JSX.Element => {
  const { calculatedInvoice, error, state } = calculationResult;

  const completedResult: JSX.Element = useMemo((): JSX.Element => {
    if (!calculatedInvoice.length) {
      return <p>No results...</p>;
    }

    return (
      <List>
        {calculatedInvoice.map((ci) => (
          <ListItemText
            key={ci.customer}
            primary={`${ci.customer} - ${outputCurrency} ${ci.total}`}
          />
        ))}
      </List>
    );
  }, [calculatedInvoice, outputCurrency]);

  const errorResult: JSX.Element = useMemo(
    (): JSX.Element => <p className={styles.error}>{error}</p>,
    [error]
  );

  return (
    <div>
      {state === DataState.pending ? (
        <CircularProgress />
      ) : state === DataState.completed ? (
        completedResult
      ) : state === DataState.error ? (
        errorResult
      ) : (
        <></>
      )}
    </div>
  );
};
