# Syntax

```abnf
doc            = assembly members
assembly       = name
name           = text
members        = *member
member         = *main-element [inheritdoc]

main-element   = summary
               / param
               / returns
               / remarks
               / typeparam

summary        = nested-elements-or-inheritdoc
param          = nested-elements-or-inheritdoc
returns        = nested-elements-or-inheritdoc
remarks        = nested-elements-or-inheritdoc
typeparam      = nested-elements-or-inheritdoc

nested-elements-or-inheritdoc = nested-elements / inheritdoc
nested-elements               = *nested-element
nested-element                = text
                              / inline-code
                              / block-code
                              / see
                              / paramref
                              / typeparamref

inline-code     = nested-elements
block-code      = nested-elements
```

