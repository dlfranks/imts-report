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
  datasetName: PrimitiveType;
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
  function renderRow(item: T, index:number) {
    return (
      <Table.Row key={Math.random() + '-' + index}>
        {objectKeys(item).map((itemProperty, index) => {
          const customRenderer = props.customRenderers?.[itemProperty];

          if (customRenderer) {
            return <Table.Cell><div>{customRenderer(item)}</div></Table.Cell>;
          }

          return (
            <Table.Cell
              style={{
                textOverflow: "ellipsis",
                overflow: "hidden",
                whiteSpace: "nowrap",
              }}
              key={Math.random() + '-' + index}
            >
              <div>{isPrimitive(item[itemProperty]) ? item[itemProperty] : ""}</div>
            </Table.Cell>
          );
        })}
      </Table.Row>
    );
  }

  return (
    <div className="container_table">
      <Table celled>
        <Table.Header>
          <Table.Row>
            {objectValues(props.headers).map((headerValue) => (
              <Table.HeaderCell key={headerValue}>
                {headerValue}
              </Table.HeaderCell>
            ))}
          </Table.Row>
        </Table.Header>
        <Table.Body>{props.items.map(renderRow)}</Table.Body>
      </Table>
    </div>
  );
}
