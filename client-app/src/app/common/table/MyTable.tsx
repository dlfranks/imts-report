import React from "react";
import { Table } from "semantic-ui-react";

/** Helpers */

// helper to get an array containing the object values with
// the correct type infered.
export function objectValues<T extends {}>(obj: T) {
  return Object.keys(obj).map((objKey) => obj[objKey as keyof T]);
}

export function objectKeys<T extends {}>(obj: T) {
  return Object.keys(obj).map((objKey) => objKey as keyof T);
}

type PrimitiveType = string | Symbol | number | boolean;

// Type guard for the primitive types which will support printing
// out of the box
export function isPrimitive(value: any): value is PrimitiveType {
  return (
    typeof value === "string" ||
    typeof value === "number" ||
    typeof value === "boolean" ||
    typeof value === "symbol"
  );
}

/** Component */

export interface MinTableItem {
  id: PrimitiveType;
}

export type TableHeaders<T extends MinTableItem> = Record<keyof T, string>;

type CustomRenderers<T extends MinTableItem> = Partial<
  Record<keyof T, (it: T) => React.ReactNode>
>;

interface TableProps<T extends MinTableItem> {
  items: T[];
  headers: TableHeaders<T>;
  customRenderers?: CustomRenderers<T>;
}

export default function MyTable<T extends MinTableItem>(props: TableProps<T>) {
  function renderRow(item: T) {
    return (
      <Table.Row>
        {objectKeys(item).map((itemProperty) => {
          const customRenderer = props.customRenderers?.[itemProperty];

          if (customRenderer) {
            return <Table.Cell >{customRenderer(item)}</Table.Cell>;
          }

          return (
            <Table.Cell style={{ 'text-overflow': 'ellipsis', 'overflow': 'hidden', 'white-space':'nowrap' }}>{isPrimitive(item[itemProperty]) ? item[itemProperty] : ""}</Table.Cell>
          );
        })}
      </Table.Row>
    );
  }

    return (
        <div className='container_table'>
          <Table celled>
            <Table.Header>
              <Table.Row>
                {objectValues(props.headers).map((headerValue) => (
                  <Table.HeaderCell key={headerValue}>{headerValue}</Table.HeaderCell>
                ))}
              </Table.Row>
            </Table.Header>
            <Table.Body>{props.items.map(renderRow)}</Table.Body>
              </Table>
        </div>
  );
}
