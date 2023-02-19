# Bargreen
Bargreen Engineering Candidate Challenge

## A Few Notes

- The data classes InventoryBalance and AccountingBalance were moved to their own source files.  This is simply a preference of mine to have classes in their own source file unless it's a file-level class.

- AccountingBalances is presumed to not contain duplicates.  If checking is desired for this table, use the directive ACCOUNTING_DUPLICATE_CHECKING.  An exception will be thrown if duplicates exist.
- ItemNumbers are considered to be case-sensitive.
- Only discrepancies are tracked in ReconcileInventoryToAccounting() and the SQL query.  Items that match values are omitted.
- An ItemNumber that exists in one table but not the other will be counted as a discrepancy.  A value of 0 will be used for the total value for the missing entry.
- The static functions in InventoryService were left untouched.  They could have been converted to an instance function but were left as static in the event that the controller was already part of a published API where those public static functions were being used.
